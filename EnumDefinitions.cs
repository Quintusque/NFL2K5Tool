using System;
using System.Collections.Generic;
using System.Text;

namespace NFL2K5Tool
{
public enum SaveType
{
Roster,
Franchise
}

/// <summary>
/// Reserved numeric ranges for the attribute-dispatch enums. PlayerOffsets is implicitly
/// "everything below AppearanceAttributesStart" and needs no explicit range here.
/// Each range has headroom for future growth without renumbering anything or touching
/// the dispatch logic in InputParser.cs. Ranges are deliberately non-overlapping and
/// closed (inclusive start and end), so checks against them don't depend on evaluation
/// order the way the old single-sentinel "attr >= AppearanceAttributes.College" test did.
/// </summary>
public static class AttributeRanges
{
public const int AppearanceAttributesStart = 200;
public const int AppearanceAttributesEnd = 260; // reserved through 260; last real member is Turtleneck = 226
public const int ContractDetailsStart = 300;
public const int ContractDetailsEnd = 320; // reserved through 320; last real member is ContractBonus = 304
}

// Addresses are based on a franchise file, not a roster file.
/// Code to map player attributes to locations 
public enum PlayerOffsets
{
/**
* College is tricky to calculate.
* College strings start at 0x7a23c with Clemson, Duke, Florida State, Georgia Tech...
* The college location looks like 4 byte ints which are all negative numbers ( like b1f7ffff; which == -2127).
* Player 0 having a clemson pointer would be: fffff7b1 @location 0xb288.
* Player 1 having a clemson pointer looks like: fffff75d @location 0xb2dc; this is 0x54 greater than what player 0's clemson pointer is.
* The second college is Duke.
* Player 0 having a Duke pointer would be: fffff7b9 @location 0xb288.
* The value difference from Clemson to Duke is 8; in fact, it looks like to increase a player's 
* college by 1 college you would add 8.
* So the following formula is one we can use to calcualte college:
* // p = integer player (duane starks is the 0th player in the base save file)
* CollegeIndex(p) = (((collegePointerVal - (0xfffff7b1)) + player * 0x54)) / 8;
*/
College=0,
PBP = 4,
Photo= 6,
PlayerType = 8, // 1 byte. Retail/prospect marker: 4 (bit 2) = real NFL/free-agent player; 0, or
// bit 4 (0x10) set = draft-class prospect (the class generator's own mark, written
// fresh over the fixed 380-slot prospect window every time the game (re)generates a
// class). Not part of mAttributeOrder/text import -- this is read internally by
// GetPlayerIndexesForTeam/GetDraftClass to tell a real prospect slot in that window
// from an empty/unused one, since retail classes are usually well under 380 players.
// Confirmed against SOFTDRINK's nfl2k5_roster_records.py (cruuz/2k-football-mod-tools).
ContractValue = 0x0A, // 16-bit LE, units = $10,000 (raw 377 = $3,770,000). Byte-offset constant only --
// dispatched through the ContractDetails enum below, not through GetAttribute/SetAttribute.
Helmet_LeftShoe_RightShoe = 0x0c, // LShoe is last 3 bits; helmet is 7th bit; RShoe is bits 4,5,6 
Turtleneck_Body_EyeBlack_Hand_Dreads = 0x18, // Shared: Skin(8), Turtleneck(bits6&7), Body(4&5), EyeBlack(3), Hand(2), Dreads(1) (most sig --> least sig )
DOB = 0x19, // Skin is shared in the second nibble at this location.
MouthPiece_LeftGlove_Sleeves_NeckRoll= 0x1c,
RightGlove_LeftWrist = 0x1d,
RightWrist_LeftElbow = 0x1e,
RightElbow = 0x1f,
JerseyNumber = 0x20, // & 0x21
FaceMask = 0x21, // part if Wacko Visor is in this byte too (FaceMask = val & 0x1F >> 1)
Face = 0x22, // part of Wacko Visor is in this byte too (Face = Val >>1 )
DevelopmentArchetype = 0x24, // Raw high nibble (bits 4-7) = profile*8+subtype (profile: bit7 0/1, subtype: bits4-6 0-7,
// retail only ever uses subtype 0-5). GetAttribute/SetAttribute expose this as a compact
// 1-12 number instead of the raw 0-15 nibble, skipping the two always-unused subtype
// slots per profile (raw 6,7,14,15): friendly = profile*6 + subtype + 1.
// 1-6 = profile 0, subtype 0-5 (every position can use these)
// 7-12 = profile 1, subtype 0-5 (only QB,WR,CB,RB,FB,TE,OLB,ILB,DT,DE use these)
// K,P,FS,SS,C,G,T only ever have friendly 1-6 in retail data.
// Confirmed three independent ways: this GetAttribute/SetAttribute code, the studio's
// roster-record byte map (which had this nibble flagged "unknown" before this was found),
// and SOFTDRINK's disassembly of the live aging routine in default.xbe.
// Low nibble (bits 0-3) is Contract Years Remaining -- byte-offset constant lives on
// ContractDetails.ContractYearsRemaining below; not a PlayerOffsets member, to avoid
// colliding with this case in GetAttribute/SetAttribute's switch (same underlying value).
YearsPro = 0x25,
ContractType = 0x26, // Low nibble (bits 0-3). High nibble (bits 4-7) is the signing bonus percent
// code (0-7 = 0%,10%...70%) -- byte-offset constant only; see ContractDetails.
ContractLength = 0x27, // Low nibble (bits 0-3). High nibble unknown/unused -- preserved, never written.
Depth = 0x29,
Weight = 0x2A, // 150 + value
Height = 0x2B, // (Inches)

Position = 0x35,
Speed,
Agility,
PassArmStrength,
Stamina,
KickPower,
Durability,
Strength,
Jumping,
Coverage,
RunRoute,//0x40
Tackle,
BreakTackle,
PassAccuracy,
PassReadCoverage,
Catch,
RunBlocking,
PassBlocking,
HoldOntoBall,
PassRush,
RunCoverage,
KickAccuracy,
Leadership = 0x4C,
PowerRunStyle,
Composure,
Scramble,//4f
Consistency,
Aggressiveness
}

/// <summary>
/// Contract type codes at PlayerOffsets.ContractType's low nibble. Names carry no spaces so the
/// friendly text editor round-trips them directly (e.g. raw 2 -> "Balanced", raw 5 -> "UpDown").
/// </summary>
public enum ContractType
{
FrontLoad = 0,
Descending,
Balanced,
Middle,
Edge,
UpDown,
Ascending,
BackLoad
}

/// <summary>
/// Values fall within AttributeRanges.AppearanceAttributesStart..AppearanceAttributesEnd (200-260).
/// College=200 starts the range with room to spare from PlayerOffsets' highest real value (0x51=81),
/// and there's headroom above Turtleneck=226 for future appearance fields before hitting 260.
/// </summary>
public enum AppearanceAttributes
{
College = 200, // starting here so that we have no collisions with the PlayerOffsets enum
DOB, YearsPro, PBP, Photo, Hand, Weight, Height, BodyType, Skin, Face, Dreads, Helmet, FaceMask, Visor,
EyeBlack, MouthPiece, LeftGlove, RightGlove, LeftWrist, RightWrist, LeftElbow,
RightElbow, Sleeves, LeftShoe, RightShoe, NeckRoll, Turtleneck
}

/// <summary>
/// Contract fields, dispatched independently from PlayerOffsets (raw ratings/skills) and
/// AppearanceAttributes (cosmetics) -- a third parallel category with its own reserved range,
/// AttributeRanges.ContractDetailsStart..ContractDetailsEnd (300-320), well clear of both of the
/// others. This is where ContractYearsRemaining and ContractBonus live as real enum members (they
/// have no PlayerOffsets equivalent, since ContractYearsRemaining would collide with
/// DevelopmentArchetype's underlying value and ContractBonus has no independent byte of its own).
/// ContractValue, ContractType, and ContractLength are listed here too, even though they also have
/// PlayerOffsets members for byte-offset math -- the PlayerOffsets versions are for locating the
/// byte; these are for generic dispatch (GetPlayerContractAttribute/SetPlayerContractAttribute),
/// the same relationship AppearanceAttributes has with the handful of PlayerOffsets-backed fields
/// it wraps (DOB, YearsPro, PBP, Photo, Weight, Height).
/// </summary>
public enum ContractDetails
{
ContractValue = 300,
ContractYearsRemaining,
ContractLength,
ContractType,
ContractBonus
}

/// enum for positions 
public enum Positions
{
QB = 0,
K,
P,
WR,
CB,
FS,
SS,
RB,
FB,
TE,
OLB,
ILB,
C,
G,
T,
DT,
DE
}

/// power run style enum 
public enum PowerRunStyle
{
Finesse = 1,
Balanced = 0x32,
Power = 0x63
}

public enum Turtleneck
{
None = 0,
White,
Black,
Team
}

public enum Body
{
Skinny = 0,
Normal,
Large,
ExtraLarge
}

public enum YesNo
{
No =0,
Yes
}

public enum Hand
{
Left=0,
Right
}

public enum Face
{
Face1=0,
Face2,
Face3,
Face4,
Face5,
Face6,
Face7,
Face8,
Face9,
Face10,
Face11,
Face12,
Face13,
Face14,
Face15
}

public enum FaceMask
{
FaceMask1 = 0,
FaceMask2,
FaceMask3,
FaceMask4,
FaceMask5,
FaceMask6,
FaceMask7,
FaceMask8,
FaceMask9,
FaceMask10,
FaceMask11,
FaceMask12,
FaceMask13,
FaceMask14,
FaceMask15,
FaceMask16,
FaceMask17,
FaceMask18,
FaceMask19,
FaceMask20,
FaceMask21,
FaceMask22,
FaceMask23,
FaceMask24,
FaceMask25,
FaceMask26,
FaceMask27
}

public enum Visor
{
None,
Dark,
Clear
}

public enum Skin
{
Skin1,
Skin2,
Skin3,
Skin4,
Skin5,
Skin6,
Skin7,
Skin8,
Skin9,
Skin10,
Skin11,
Skin12,
Skin13,
Skin14,
Skin15,
Skin16,
Skin17,
Skin18,
Skin19,
Skin20,
Skin21,
Skin22
}

public enum Helmet
{
Standard =0,
Revolution
}

public enum Shoe
{
Shoe1,
Shoe2,
Shoe3,
Shoe4,
Shoe5,
Shoe6,
Taped
}

public enum Glove
{
None,
Type1,
Type2,
Type3,
Type4,
Team1,
Team2,
Team3,
Team4,
Taped
}

public enum Sleeves
{
None,
White,
Black,
Team
}

public enum NeckRoll
{
None,
Collar,
Roll,
Washboard,
Bulging
}

public enum Wrist
{
None,
SingleWhite,
DoubleWhite,
SingleBlack,
DoubleBlack,
NeopreneSmall,
NeopreneLarge,
ElasticSmall,
ElasticLarge,
SingleTeam,
DoubleTeam,
TapedSmall,
TapedLarge,
Quarterback
}

public enum Elbow
{
None,
White,
Black,
WhiteBlackStripe,
BlackWhiteStripe,
BlackTeamStripe,
Team,
WhiteTeamStripe,
Elastic,
Neoprene,
WhiteTurf,
BlackTurf,
Taped,
HighWhite,
HighBlack,
HighTeam
}

public enum Game
{
HomeTeam,
AwayTeam,
Month,
Day,
YearTwoDigit,
HourOfDay,
MinuteOfHour,
NullByte
}

public enum SpecialTeamer
{
KR1 = 0x195,
KR2 = 0x196,
LS = 0x198,
PR = 0x199
}

public enum CoachOffsets
{
FirstName = 0x0,
LastName = 0x4,
Info1 = 0x8,
Info2 = 0xC,
Info3 = 0x10,
Body = 0x18,
Wins = 0x20,
Losses = 0x22,
Ties = 0x24,
SeasonsWithTeam = 0x1C,
totalSeasons = 0x1E,
WinningSeasons = 0x30,
SuperBowls = 0x32,
SuperBowlWins = 0x38,
SuperBowlLosses = 0x3A,
PlayoffWins = 0x34,
PlayoffLosses = 0x36,

Photo = 0x40, // where do you see this stuff anyways?
Overall = 0x42,
OvrallOffense = 0x43,
RushFor = 0x44,
PassFor = 0x45,
OverallDefense = 0x46,
PassRush = 0x47,
PassCoverage = 0x48,
QB = 0x49,
RB = 0x4A,
TE = 0x4B,
WR = 0x4C,
OL = 0x4D,
DL = 0x4E,
LB = 0x4F,
DB = 0x50,
SpecialTeams = 0x51,
Professionalism = 0x52,
Preparation = 0x53,
Conditioning = 0x54,
Motivation = 0x55,
Leadership = 0x56,
Discipline = 0x57,
Respect = 0x58,

// Play tendency
PlaycallingRun = 0x59,
ShotgunRun = 0x83,
IFormRun = 0x83,
SplitbackRun = 0x87,
EmptyRun = 0x87,
ShotgunPass = 0x88,
SplitbackPass = 0x89,
IFormPass = 0x8A,
LoneBackPass = 0x8B,
EmptyPass = 0x8C
}

/// <summary>
/// Team-record fields for the team's own data (nickname/city/stadium/etc.), distinct from
/// CoachOffsets. Base for all of these is the SAME team block GetCoachPointer already uses:
/// m49ersPlayerPointersStart + teamIndex * cTeamDiff.
///
/// Ported from BadAL's nfl2k5tool_dart (lib/enum_definitions.dart + lib/gamesave_tool.dart),
/// whose byte offsets were verified test-by-test against real save files, and independently
/// re-verified here against this project's own real franchise save on 2026-09-26 (all 32
/// teams' Nickname/Abbrev/City/AbbrAlt/Logo/Stadium bytes matched exactly).
///
/// DELIBERATELY left as a plain ordinal enum (no explicit int values), matching the Dart
/// source exactly -- unlike PlayerOffsets/CoachOffsets, these members do NOT share one
/// consistent "underlying value = byte offset" meaning (Nickname/Abbrev/City/AbbrAlt are
/// string-block indices; Stadium/Logo/DefaultJersey are byte offsets; Playbook is a separate
/// pointer-table lookup with no offset at all). Giving members mismatched explicit values
/// here previously produced silently-wrong auto-incremented offsets for City/AbbrAlt/
/// Playbook -- do not add explicit int values to this enum. The real offsets/mechanisms
/// live as named constants in GamesaveTool.cs and are selected via a switch on the enum
/// member itself (mirroring the Dart GetTeamString/SetTeamString switch), never via casting
/// this enum to int.
///
/// Per-member mechanism (see GamesaveTool.cs for the actual constants/methods):
///
///   Nickname, Abbrev, City, AbbrAlt:
///     teamBlock+0x104 holds a signed 32-bit relative POINTER (same formula as player/coach
///     name pointers: dest = ptrLoc + value - 1) to a block of 5 sequential UTF-16LE
///     null-terminated strings: [0]=Nickname [1]=Abbrev [2]=LogoNumStr(2-char decimal,
///     internal-only, kept in sync with Logo) [3]=City [4]=AbbrAlt. Each field's address is
///     found by walking forward from the previous string's length. Writing a value LONGER
///     than the current string must be REJECTED (no shift/pointer-adjust logic exists for
///     this block, unlike player/coach names). Writing a value SHORTER is allowed and must
///     be right-padded with spaces to preserve the original byte length exactly (confirmed in
///     Dart source and by this project's relocation-mod save, where "San Diego"->"L.Angeles",
///     "Oakland"->"L.Vegas", "St. Louis"->"L.Angeles" are all exact same-length swaps).
///
///   Stadium (teamBlock+0x118, 1 byte):
///     0-based INDEX (not a string) into a stadium name/city lookup table built once at load
///     time by scanning the S1a stadium block. Read/write via dedicated
///     GetStadiumName(ByIndex)/SetStadiumIndex methods, not GetAttribute/SetAttribute. Text
///     format wraps the resolved name in [brackets], e.g. "[San Francisco Park]". Unknown
///     stadium name on write = error.
///
///   Logo (teamBlock+0x154, 1 byte):
///     Index numerically identical to Stadium's index for every standard NFL team (verified
///     on all 32 teams against this project's real save), tracked/settable independently via
///     SetLogoIndex, which also keeps the 2-char zero-padded LogoNumStr at S3a[2] in sync.
///     Sits immediately after DefScheme (0x150) + 3 zero padding bytes (0x151-0x153) --
///     adjacent, non-overlapping fields, confirmed by real save bytes. Non-integer value on
///     write = error.
///
///   Playbook:
///     NOT part of the team block. A separate absolute table of 32 x 8-byte entries: table
///     base = 0x2C90 (franchise) / 0x29B0 (roster), stride 0x8 per team. Each entry is two
///     signed 32-bit relative string pointers: +0x0 = full playbook name (e.g. "49ers",
///     "West Coast"), +0x4 = short abbreviation (e.g. "SF", "WCO"). Text value format is
///     "PB_" + name with spaces replaced by "_" (e.g. "PB_49ers", "PB_West_Coast", plus 4
///     generic entries: PB_West_Coast, PB_General, PB_User_A, PB_User_B). Get/Set resolves/
///     writes BOTH pointers atomically from a single Playbook value, via a name->address
///     lookup built once at load by walking the string table forward from team 0's offense
///     pointer address. Unknown playbook token on write = error.
///
///   DefaultJersey (teamBlock+0x192, 1 byte):
///     0-based index into that TEAM'S OWN jersey list (variable length per team). Non-integer
///     value on write = error. An index in-range for the byte but out of range for that
///     team's own jersey list is a WARNING, not an error -- the value is still written
///     (matches BadAL's SetTeamString behavior; do not silently reject or clamp it).
/// </summary>
public enum TeamDataOffsets
{
Nickname,
Abbrev,
Stadium,
City,
AbbrAlt,
Logo,
Playbook,
DefaultJersey
}

/// <summary>
/// Default defensive front/scheme, stored as a single byte at teamBlock+0x150 -- immediately
/// before Logo (0x154), with bytes 0x151-0x153 confirmed always zero (padding) across every
/// team in a real save. Sourced from SOFTDRINK's cruuz/2k-football-mod-tools byte map
/// (TEAM_SCHEME_WORD) and independently verified byte-for-byte against this project's own
/// real franchise save on 2026-09-25: all 32 teams' values (23x Scheme43, 5x Scheme34,
/// 4x SchemeDual) matched exactly, with zero padding bytes in every case.
/// SchemeDual is played by the game as a 4-3 front; it is a distinct value on disk but not a
/// distinct AI behavior from Scheme43 as far as the native front-selection routine
/// (FUN_000c40f0's ILB/DT chain rule) is concerned. These ARE meaningful as raw byte values
/// (unlike TeamDataOffsets above), since the whole point of this enum is to be cast directly
/// to/from the single byte at +0x150.
/// </summary>
public enum DefensiveScheme
{
Scheme43 = 0,
Scheme34 = 1,
SchemeDual = 2
}

public enum FormulaMode
{
Normal = 0,
Add,
Percent
}
}
