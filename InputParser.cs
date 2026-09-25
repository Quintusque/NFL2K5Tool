// ===================================================================================
// InputParser.cs additions for Team Data editing.
// Mirrors the existing CoachKEY=/Coach, dispatch pattern exactly.
//
// INTEGRATION NOTES:
//
// 1. In ProcessLine(string line), add two new branches to the existing if/else-if
//    chain, positioned right next to the CoachKEY=/Coach, handling (order relative to
//    the other branches doesn't matter functionally, but keeping it next to Coach's
//    handling keeps the two team-scoped text formats together for readability):
//
//        else if (line.StartsWith("CoachKEY", StringComparison.InvariantCultureIgnoreCase))
//            Tool.CoachKey = line.Substring(9);
//        else if (line.StartsWith("TeamDataKEY", StringComparison.InvariantCultureIgnoreCase))   // NEW
//            Tool.TeamDataKey = line.Substring(12);                                               // NEW
//        ...
//        else if (line.StartsWith("Coach,", StringComparison.InvariantCultureIgnoreCase))
//            SetCoachData(line);
//        else if (line.StartsWith("TeamData,", StringComparison.InvariantCultureIgnoreCase))      // NEW
//            SetTeamData(line);                                                                    // NEW
//
//    "TeamDataKEY=" is 12 characters, same reasoning as "CoachKEY=" being 9 -- both
//    Substring calls strip the "KEY=" prefix (including the '=') to hand the key-parts
//    string straight to the property setter, which already validates each part via
//    Enum.Parse in GamesaveTool.TeamDataKey's setter.
//
// 2. Add the SetTeamData(string) private method below to the class body, near the
//    existing SetCoachData(string) method.
// ===================================================================================

/// <summary>
/// Applies a "TeamData,Team,val1,val2,..." line using the current TeamDataKey to map
/// each value to its TeamDataOffsets field. Mirrors SetCoachData(string) exactly --
/// same "walk keyParts from index 2, stop at whichever list runs out first, catch and
/// report on the specific field that failed" structure.
///
/// Deliberately uses a plain line.Split(',') rather than ParseCoachLine's quote-aware
/// splitting: no TeamData field (team names, nicknames, city names, stadium brackets,
/// playbook tokens, integer indices) is ever expected to contain a literal comma, so
/// the extra quote-handling logic ParseCoachLine needs for coach Info fields has no
/// equivalent need here. Matches BadAL's nfl2k5tool_dart _SetTeamDataLine, which uses
/// the same plain split for the same reason.
/// </summary>
/// <param name="line">A line like "TeamData,49ers,49ers,SF,[San Francisco Park],San Francisco,SF,25,PB_49ers,0"</param>
private void SetTeamData(string line)
{
    string[] keyParts = Tool.TeamDataKey.Split(",".ToCharArray());
    string[] parts = line.Split(",".ToCharArray());
    if (parts.Length < 2)
        return;

    int teamIndex = Tool.GetTeamIndex(parts[1]);
    if (teamIndex < 0)
    {
        StaticUtils.AddError(String.Format("TeamData: unknown team '{0}' in line: {1}", parts[1], line));
        return;
    }

    TeamDataOffsets current = TeamDataOffsets.Nickname;
    try
    {
        for (int i = 2; i < keyParts.Length; i++)
        {
            if (i >= parts.Length) break; // stop processing if we're out of parts, same as SetCoachData
            string lp = keyParts[i].ToLower();
            if (lp == "teamdata" || lp == "team") continue; // header/team columns handled above, skip if repeated

            current = (TeamDataOffsets)Enum.Parse(typeof(TeamDataOffsets), keyParts[i], true);
            // Strip brackets for Stadium, matching the coach Body convention
            // (SetCoachAttribute's Body case also strips brackets before lookup).
            string val = parts[i].Replace("[", "").Replace("]", "");
            Tool.SetTeamString(teamIndex, current, val);
        }
    }
    catch (Exception)
    {
        StaticUtils.AddError(String.Format(
            "Error setting data for line:'{0}' check '{1}' attribute.", line, current.ToString()));
    }
}
