// ===================================================================================
// GamesaveTool.cs additions for Team Data editing.
// Ported from BadAL's nfl2k5tool_dart (lib/gamesave_tool.dart), commits 6f4c19a and
// dafe7ca, cross-verified against this project's own real franchise save on 2026-09-26.
//
// INTEGRATION NOTES (apply these edits to the existing file):
//
// 1. Add these fields near the other team-block constants (m49ersPlayerPointersStart,
//    cTeamDiff, mCoachPointerOffset):
//
//        private const int cTeamDataPtrOffset      = 0x104; // team_block -> S3a nickname ptr
//        private const int cTeamStadiumByteOffset  = 0x118; // team_block -> stadium index byte
//        private const int cTeamDefSchemeOffset    = 0x150; // team_block -> DefScheme byte (verified)
//        private const int cTeamLogoByteOffset     = 0x154; // team_block -> logo/PBP index byte
//        private const int cTeamDefaultJerseyOffset= 0x192; // team_block -> default jersey index byte
//        private int mPlaybookTableBase = 0x2C90;           // absolute; stride 8/team; set per save type
//
//        private Dictionary<int, int>    mStadiumShortNameAddresses = new Dictionary<int, int>();
//        private Dictionary<string, int> mStadiumNameToIndex        = new Dictionary<string, int>();
//        private Dictionary<string, int> mPlaybookOffAddr = new Dictionary<string, int>();
//        private Dictionary<string, int> mPlaybookDefAddr = new Dictionary<string, int>();
//
// 2. In InitializeForFranchise(), add:  mPlaybookTableBase = 0x2C90;
//    In InitializeForRoster(),    add:  mPlaybookTableBase = 0x29B0;
//
// 3. In SetupForSaveType(), after the existing Initialize...() call, add:
//        BuildStadiumLookup();
//        BuildPlaybookLookup();
//    (Both scan the file once at load time; must run AFTER mStringTableStart/
//    m49ersPlayerPointersStart are set for the correct save type.)
//
// 4. Paste all the methods below into the class body (grouped here under one region
//    for clarity; feel free to split across the existing Coach/Team regions as you see fit).
// ===================================================================================

#region Team Data (S3a strings, S1a stadiums, Playbook table, DefScheme)

/// <summary>
/// Scan S1a once at load time to build the stadium index <-> address maps.
/// Each S1a entry is: short_name, city, "sNN" code, long_name -- repeating.
/// We only need the short name (two strings back from the "sNN" code).
/// </summary>
private void BuildStadiumLookup()
{
    mStadiumShortNameAddresses.Clear();
    mStadiumNameToIndex.Clear();

    // S1a ends where S2 begins -- the address of coach 0's FirstName string.
    int s2Start = GetPointerDestination(GetPointerDestination(GetCoachPointer(0)));

    int i = mStringTableStart;
    string prevStr = "", prevPrevStr = "";
    int prevAddr = 0, prevPrevAddr = 0;

    while (i < s2Start - 1)
    {
        while (i < s2Start - 1 && GameSaveData[i] == 0)
            i += 2;
        if (i >= s2Start - 1) break;

        int strStart = i;
        StringBuilder sb = new StringBuilder();
        while (i < s2Start && GameSaveData[i] != 0)
        {
            sb.Append((char)GameSaveData[i]);
            i += 2;
        }
        string s = sb.ToString();
        if (s.Length == 0) { i += 2; continue; }

        i += 2; // step over null terminator

        // A 3-char "sNN" code means two strings back is the stadium short name.
        if (s.Length == 3 && s[0] == 's')
        {
            int idx;
            if (Int32.TryParse(s.Substring(1), out idx) && prevPrevStr.Length > 0)
            {
                mStadiumShortNameAddresses[idx] = prevPrevAddr;
                mStadiumNameToIndex[prevPrevStr] = idx;
            }
        }

        prevPrevStr = prevStr; prevPrevAddr = prevAddr;
        prevStr = s; prevAddr = strStart;
    }
}

/// <summary>
/// Scan the 32-team playbook pointer table to build name/abbrev -> address maps.
/// Silently no-ops if the table doesn't resolve (should not happen for a valid save,
/// but mirrors the Dart port's defensive bounds checks).
/// </summary>
private void BuildPlaybookLookup()
{
    mPlaybookOffAddr.Clear();
    mPlaybookDefAddr.Clear();
    int len = GameSaveData.Length;

    int firstPtrLoc = mPlaybookTableBase;
    if (firstPtrLoc + 4 > len) return;
    int addr = GetPointerDestination(firstPtrLoc);
    if (addr < 0 || addr >= len) return;

    // Walk (offense-name, defense-abbrev) pairs. A valid defense abbreviation is
    // short (<=4 chars), all-uppercase, non-empty; the first string that fails
    // that test ends the block.
    while (addr < len)
    {
        int offAddr = addr;
        string offName = GetString(offAddr);
        if (offName.Length == 0) break;
        int defAddr = offAddr + offName.Length * 2 + 2;
        if (defAddr + 2 > len) break;
        string defAbbrev = GetString(defAddr);
        if (defAbbrev.Length == 0 || defAbbrev.Length > 4 || defAbbrev != defAbbrev.ToUpper())
            break;
        mPlaybookOffAddr[offName] = offAddr;
        mPlaybookDefAddr[offName] = defAddr;
        addr = defAddr + defAbbrev.Length * 2 + 2;
    }
}

private static string PlaybookNameToToken(string name)
{
    return "PB_" + name.Replace(" ", "_");
}

private static string PlaybookTokenToName(string token)
{
    return token.StartsWith("PB_") ? token.Substring(3).Replace("_", " ") : token;
}

private void WritePointerToAddr(int ptrLoc, int targetAddr)
{
    int pointer = targetAddr - ptrLoc + 1;
    SetByte(ptrLoc, (byte)(pointer & 0xff));
    SetByte(ptrLoc + 1, (byte)((pointer >> 8) & 0xff));
    SetByte(ptrLoc + 2, (byte)((pointer >> 16) & 0xff));
    SetByte(ptrLoc + 3, (byte)((pointer >> 24) & 0xff));
}

/// <summary>
/// Returns the index into the S3a string block (0-4) for the given attr.
/// Throws for attrs with no S3a field (Stadium, Logo, Playbook, DefaultJersey).
/// </summary>
private int TeamDataOffsetToS3aIndex(TeamDataOffsets attr)
{
    switch (attr)
    {
        case TeamDataOffsets.Nickname: return 0;
        case TeamDataOffsets.Abbrev: return 1;
        case TeamDataOffsets.City: return 3;
        case TeamDataOffsets.AbbrAlt: return 4;
        default:
            throw new ArgumentException(attr + " has no S3a field index");
    }
}

/// <summary>
/// Returns the team block's base address: m49ersPlayerPointersStart + teamIndex * cTeamDiff.
/// Same base GetCoachPointer already uses.
/// </summary>
private int GetTeamBlock(int teamIndex)
{
    return teamIndex * cTeamDiff + m49ersPlayerPointersStart;
}

/// <summary>
/// Returns the address in the S3a string block of the field at s3aFieldIndex for teamIndex.
/// Walks forward from the Nickname pointer's destination, since fields are packed
/// sequentially (not fixed-width slots).
/// </summary>
private int GetS3aStringAddress(int teamIndex, int s3aFieldIndex)
{
    int teamBlock = GetTeamBlock(teamIndex);
    int addr = GetPointerDestination(teamBlock + cTeamDataPtrOffset);
    for (int f = 0; f < s3aFieldIndex; f++)
    {
        string s = GetString(addr);
        addr += s.Length * 2 + 2; // string + null terminator
    }
    return addr;
}

public string GetStadiumNameByIndex(int stadiumIndex)
{
    if (!mStadiumShortNameAddresses.ContainsKey(stadiumIndex))
        return "!!!!Invalid!!!!";
    return GetString(mStadiumShortNameAddresses[stadiumIndex]);
}

public string GetStadiumName(int teamIndex)
{
    if (teamIndex < 0 || teamIndex >= 32) return "!!!!Invalid!!!!";
    int teamBlock = GetTeamBlock(teamIndex);
    int idx = GameSaveData[teamBlock + cTeamStadiumByteOffset];
    return GetStadiumNameByIndex(idx);
}

/// <summary>
/// All known stadium names, sorted by index, one per line: "  NN: [Name]".
/// Useful for populating a dropdown or CLI listing of valid Stadium values.
/// </summary>
public string GetStadiumNamesList()
{
    StringBuilder sb = new StringBuilder("\nStadium names:\n");
    List<int> indices = new List<int>(mStadiumShortNameAddresses.Keys);
    indices.Sort();
    foreach (int idx in indices)
        sb.Append(String.Format("  {0,2}: [{1}]\n", idx, GetStadiumNameByIndex(idx)));
    return sb.ToString();
}

/// <summary>
/// All known playbook names, one per line, as PB_ tokens.
/// </summary>
public string GetPlaybookNamesList()
{
    StringBuilder sb = new StringBuilder("\nPlaybook names:\n");
    foreach (string name in mPlaybookOffAddr.Keys)
        sb.Append("  " + PlaybookNameToToken(name) + "\n");
    return sb.ToString();
}

/// <summary>
/// Returns the jersey name for teamIndex at jerseyIndex, or null if either index is out
/// of range for that team.
/// </summary>
public string GetJerseyName(int teamIndex, int jerseyIndex)
{
    if (teamIndex < 0 || teamIndex >= 32) return null;
    string[] list;
    if (!TeamJerseyData.Names.TryGetValue(sTeamsDataOrder[teamIndex], out list)) return null;
    if (jerseyIndex < 0 || jerseyIndex >= list.Length) return null;
    return list[jerseyIndex];
}

/// <summary>
/// All jersey names for teamIndex, one per line: "  N: Jersey Name".
/// </summary>
public string GetJerseyNamesList(int teamIndex)
{
    if (teamIndex < 0 || teamIndex >= 32) return "";
    string[] list;
    if (!TeamJerseyData.Names.TryGetValue(sTeamsDataOrder[teamIndex], out list))
        list = new string[0];
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < list.Length; i++)
        sb.Append(String.Format("  {0}: {1}\n", i, list[i]));
    return sb.ToString();
}

/// <summary>
/// Reads a TeamDataOffsets field for teamIndex. See TeamDataOffsets' XML doc in
/// EnumDefinitions.cs for the full per-field mechanism.
/// </summary>
public string GetTeamString(int teamIndex, TeamDataOffsets attr)
{
    if (teamIndex < 0 || teamIndex >= 32) return "!!!!Invalid!!!!";
    int teamBlock = GetTeamBlock(teamIndex);
    switch (attr)
    {
        case TeamDataOffsets.Stadium:
            return GetStadiumName(teamIndex);
        case TeamDataOffsets.Logo:
            return "" + GameSaveData[teamBlock + cTeamLogoByteOffset];
        case TeamDataOffsets.DefaultJersey:
            return "" + GameSaveData[teamBlock + cTeamDefaultJerseyOffset];
        case TeamDataOffsets.Playbook:
            return PlaybookNameToToken(GetString(GetPointerDestination(mPlaybookTableBase + teamIndex * 8)));
        default:
            return GetString(GetS3aStringAddress(teamIndex, TeamDataOffsetToS3aIndex(attr)));
    }
}

public void SetStadiumIndex(int teamIndex, int stadiumIndex)
{
    if (teamIndex < 0 || teamIndex >= 32) return;
    if (!mStadiumShortNameAddresses.ContainsKey(stadiumIndex))
    {
        StaticUtils.AddError("SetStadiumIndex: unknown stadium index " + stadiumIndex);
        return;
    }
    int teamBlock = GetTeamBlock(teamIndex);
    SetByte(teamBlock + cTeamStadiumByteOffset, (byte)stadiumIndex);
    // For standard NFL teams, stadium index equals logo/PBP index -- keep in sync.
    SetLogoIndex(teamIndex, stadiumIndex);
}

public void SetLogoIndex(int teamIndex, int logoIndex)
{
    if (teamIndex < 0 || teamIndex >= 32) return;
    int teamBlock = GetTeamBlock(teamIndex);
    SetByte(teamBlock + cTeamLogoByteOffset, (byte)logoIndex);
    // Keep S3a[2] logo string in sync (always 2 chars -- same-length safe).
    int numAddr = GetS3aStringAddress(teamIndex, 2);
    string padded = logoIndex.ToString().PadLeft(2, '0');
    for (int i = 0; i < 2; i++)
    {
        SetByte(numAddr + i * 2, (byte)padded[i]);
        SetByte(numAddr + i * 2 + 1, 0);
    }
}

/// <summary>
/// Writes a TeamDataOffsets field for teamIndex. Enforces the real validation rules
/// (see EnumDefinitions.cs TeamDataOffsets doc comment):
///   - Nickname/Abbrev/City/AbbrAlt: reject if longer than current value; pad with
///     spaces (not an error) if shorter.
///   - Stadium/Playbook: reject unknown name/token.
///   - Logo: reject non-integer.
///   - DefaultJersey: reject non-integer; WARN (do not reject) if out of range for
///     that team's own jersey list.
/// </summary>
public void SetTeamString(int teamIndex, TeamDataOffsets attr, string value)
{
    if (teamIndex < 0 || teamIndex >= 32) return;
    int teamBlock = GetTeamBlock(teamIndex);

    switch (attr)
    {
        case TeamDataOffsets.Stadium:
            {
                int idx;
                if (!mStadiumNameToIndex.TryGetValue(value, out idx))
                {
                    StaticUtils.AddError(String.Format(
                        "SetTeamString: unknown stadium '{0}' for {1}", value, sTeamsDataOrder[teamIndex]));
                    return;
                }
                SetStadiumIndex(teamIndex, idx);
                return;
            }
        case TeamDataOffsets.Logo:
            {
                int intVal;
                if (!Int32.TryParse(value.Trim(), out intVal))
                {
                    StaticUtils.AddError(String.Format(
                        "SetTeamString: Logo must be an integer, got '{0}'", value));
                    return;
                }
                SetLogoIndex(teamIndex, intVal);
                return;
            }
        case TeamDataOffsets.DefaultJersey:
            {
                int intVal;
                if (!Int32.TryParse(value.Trim(), out intVal))
                {
                    StaticUtils.AddError(String.Format(
                        "SetTeamString: DefaultJersey must be an integer, got '{0}'", value));
                    return;
                }
                string[] jerseyList;
                if (TeamJerseyData.Names.TryGetValue(sTeamsDataOrder[teamIndex], out jerseyList)
                    && (intVal < 0 || intVal >= jerseyList.Length))
                {
                   // was: StaticUtils.AddError(String.Format("Warning: DefaultJersey index {0} out of range..."));
                    StaticUtils.AddWarning(String.Format(
                        "DefaultJersey index {0} out of range for {1} (valid: 0-{2}); value written anyway.",
                        intVal, sTeamsDataOrder[teamIndex], jerseyList.Length - 1));
                }
                SetByte(teamBlock + cTeamDefaultJerseyOffset, (byte)intVal);
                return;
            }
        case TeamDataOffsets.Playbook:
            {
                string name = PlaybookTokenToName(value);
                if (!mPlaybookOffAddr.ContainsKey(name) || !mPlaybookDefAddr.ContainsKey(name))
                {
                    List<string> known = new List<string>();
                    foreach (string k in mPlaybookOffAddr.Keys) known.Add(PlaybookNameToToken(k));
                    StaticUtils.AddError(String.Format(
                        "SetTeamString: unknown playbook '{0}' for {1}. Known: {2}",
                        value, sTeamsDataOrder[teamIndex], String.Join(", ", known.ToArray())));
                    return;
                }
                WritePointerToAddr(mPlaybookTableBase + teamIndex * 8, mPlaybookOffAddr[name]);
                WritePointerToAddr(mPlaybookTableBase + teamIndex * 8 + 4, mPlaybookDefAddr[name]);
                return;
            }
        default:
            {
                int addr = GetS3aStringAddress(teamIndex, TeamDataOffsetToS3aIndex(attr));
                string curr = GetString(addr);
                if (value.Length > curr.Length)
                {
                    StaticUtils.AddError(String.Format(
                        "SetTeamString: value too long for {0} {1}: '{2}'({3}) -> '{4}'({5}). Maximum length is {3}.",
                        sTeamsDataOrder[teamIndex], attr, curr, curr.Length, value, value.Length));
                    return;
                }
                // Shorter values are right-padded with spaces to preserve the on-disk string length.
                if (value.Length < curr.Length)
                    value = value.PadRight(curr.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    SetByte(addr + i * 2, (byte)value[i]);
                    SetByte(addr + i * 2 + 1, 0);
                }
                return;
            }
    }
}

/// <summary>
/// The default defensive front/scheme for teamIndex. See DefensiveScheme's XML doc
/// in EnumDefinitions.cs -- this is a genuinely new field, verified byte-accurate
/// against a real save (not ported from either BadAL tool).
/// </summary>
public DefensiveScheme GetDefScheme(string team)
{
    int teamIndex = GetTeamIndex(team);
    return (DefensiveScheme)GameSaveData[GetTeamBlock(teamIndex) + cTeamDefSchemeOffset];
}

public void SetDefScheme(string team, DefensiveScheme scheme)
{
    int teamIndex = GetTeamIndex(team);
    SetByte(GetTeamBlock(teamIndex) + cTeamDefSchemeOffset, (byte)scheme);
}

// ─── Text export / TeamDataKey (mirrors CoachKey/GetCoachData exactly) ─────────────

public const string DefaultTeamDataKey = "TeamData,Team,Nickname,Abbrev,Stadium,City,AbbrAlt";

private string mTeamDataKeyAll =
    "TeamData,Team,Nickname,Abbrev,Stadium,City,AbbrAlt,Logo,Playbook,DefaultJersey";

public string TeamDataKeyAll { get { return mTeamDataKeyAll; } }

private string mTeamDataKey = DefaultTeamDataKey;

public string TeamDataKey
{
    get { return mTeamDataKey; }
    set
    {
        string lastAttr = "";
        try
        {
            string[] parts = value.Split(",".ToCharArray());
            foreach (string part in parts)
            {
                if (part.Length > 0
                    && !part.Equals("Team", StringComparison.InvariantCultureIgnoreCase)
                    && !part.Equals("TeamData", StringComparison.InvariantCultureIgnoreCase))
                {
                    lastAttr = part;
                    Enum.Parse(typeof(TeamDataOffsets), part, true); // throws on invalid part
                }
            }
            mTeamDataKey = value;
        }
        catch (Exception)
        {
            StaticUtils.AddError(String.Format("Error setting TeamDataKey part='{0}' in '{1}'", lastAttr, value));
        }
    }
}

/// <summary>
/// "TeamData,Team,..." one row for teamIndex, mirrors GetCoachData(int).
/// Stadium is wrapped in [brackets] in text output; all other fields are plain values.
/// </summary>
public string GetTeamData(int teamIndex)
{
    if (teamIndex < 0 || teamIndex >= 32) return "";
    StringBuilder sb = new StringBuilder("TeamData,");
    sb.Append(sTeamsDataOrder[teamIndex]);
    sb.Append(",");
    string[] parts = TeamDataKey.Split(",".ToCharArray());
    foreach (string part in parts)
    {
        if ("TeamData,Team".IndexOf(part, StringComparison.InvariantCultureIgnoreCase) == -1)
        {
            TeamDataOffsets attr = (TeamDataOffsets)Enum.Parse(typeof(TeamDataOffsets), part, true);
            string val = GetTeamString(teamIndex, attr);
            if (attr == TeamDataOffsets.Stadium)
            {
                sb.Append("[");
                sb.Append(val);
                sb.Append("],");
            }
            else
            {
                sb.Append(val);
                sb.Append(",");
            }
        }
    }
    sb.Remove(sb.Length - 1, 1); // remove trailing comma
    return sb.ToString();
}

/// <summary>
/// "TeamDataKEY=..." header + one TeamData, row per team, mirrors GetCoachData().
/// </summary>
public string GetTeamDataAll()
{
    StringBuilder sb = new StringBuilder(1000);
    sb.Append("\n\nTeamDataKEY=");
    sb.Append(this.TeamDataKey);
    sb.Append("\n");
    for (int i = 0; i < 32; i++)
    {
        sb.Append(this.GetTeamData(i));
        sb.Append("\r\n");
    }
    return sb.ToString();
}

#endregion
