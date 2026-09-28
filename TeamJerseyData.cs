using System.Collections.Generic;

namespace NFL2K5Tool
{
    /// <summary>
    /// Team name -> ordered list of selectable jersey names (index 0 = default).
    /// Direct port of BadAL's nfl2k5tool_dart lib/jersey_data.dart (kTeamJerseyNames),
    /// itself auto-generated from uniform_data.txt -- do not edit by hand.
    /// Used by GamesaveTool.GetJerseyName/GetJerseyNamesList and by
    /// SetTeamDataAttribute's DefaultJersey range-check warning.
    /// </summary>
    public static class TeamJerseyData
    {
        public static readonly Dictionary<string, string[]> Names = new Dictionary<string, string[]>()
        {
            { "49ers", new[] {
                "1998 - 2004 Uniform", "1996 - 1997 Uniform", "1993 - 1994 Uniform",
                "1964 - 1995 Uniform", "1962 - 1963 Uniform", "1960 - 1961 Uniform",
                "1958 - 1959 Uniform", "2004 Alternate 1"
            }},
            { "Bears", new[] {
                "1995 - 2004 Uniform", "1974 - 1982 Uniform", "1962 - 1973 Uniform",
                "1958 - 1961 Uniform", "2003 Alternate 1", "2003 Alternate 2", "2004 Alternate 1"
            }},
            { "Bengals", new[] {
                "1998 - 2004 Uniform", "1981 - 1997 Uniform", "1979 - 1980 Uniform",
                "1968 - 1978 Uniform", "2004 Alternate 1", "2004 Alternate 2"
            }},
            { "Bills", new[] {
                "2002 - 2004 Uniform", "1993 - 1994 Uniform", "1987 - 2002 Uniform",
                "1984 - 1986 Uniform", "1976 - 1983 Uniform", "1974 - 1975 Uniform",
                "1969 - 1973 Uniform", "1964 - 1965 Uniform", "2003 Alternate 1", "2003 Alternate 2"
            }},
            { "Broncos", new[] {
                "1997 - 2004 Uniform", "1985 - 1996 Uniform", "1969 - 1973 Uniform",
                "1963 - 1965 Uniform", "1961 - 1962 Uniform", "2003 Alternate 1", "2003 Alternate 2"
            }},
            { "Browns", new[] {
                "1999 - 2004 Uniform", "1983 - 1984 Uniform", "1964 - 1974 Uniform",
                "2003 Alternate 1", "2003 Alternate 2"
            }},
            { "Buccaneers", new[] {
                "1997 - 2004 Uniform", "1993 - 1994 Uniform", "1992 - 1996 Uniform",
                "1976 - 1991 Uniform", "1997 Alternate 1", "1997 Alternate 2",
                "2004 Alternate 1", "2004 Alternate 2"
            }},
            { "Cardinals", new[] {
                "1998 - 2004 Uniform", "1993 - 1994 Uniform", "1990 - 1991 Uniform",
                "1971 - 1972 Uniform", "1959 - 1960 Uniform", "1958 - 1959 Uniform",
                "2003 Alternate 1", "2003 Alternate 2", "2004 Alternate 1"
            }},
            { "Chargers", new[] {
                "1989 - 2004 Uniform", "1988 - 1989 Uniform", "1985 - 1987 Uniform",
                "1983 - 1984 Uniform", "1978 - 1979 Uniform", "1973 - 1974 Uniform",
                "1967 - 1972 Uniform", "1965 - 1966 Uniform", "1963 - 1965 Uniform",
                "2003 Alternate 1", "2003 Alternate 2", "2004 Alternate 1"
            }},
            { "Chiefs", new[] {
                "2000 - 2004 Uniform", "1993 - 1994 Uniform", "1991 - 1993 Uniform",
                "1970 - 1971 Uniform", "1963 - 1970 Uniform", "1960 - 1962 Uniform"
            }},
            { "Colts", new[] {
                "2001 - 2004 Uniform", "1995 - 1995 Uniform", "1988 - 1991 Uniform",
                "1987 - 1987 Uniform", "1982 - 1982 Uniform", "1968 - 1970 Uniform",
                "1955 - 1956 Uniform", "1954 - 1955 Uniform"
            }},
            { "Cowboys", new[] {
                "1997 - 2004 Uniform", "1993 - 1995 Uniform", "1982 - 1994 Uniform",
                "1977 - 1980 Uniform", "1975 - 1976 Uniform", "1964 - 1966 Uniform",
                "1960 - 1963 Uniform", "1994 Alternate 1", "1994 Alternate 2", "2002 Alternate 1"
            }},
            { "Dolphins", new[] {
                "2000 - 2004 Uniform", "1993 - 1994 Uniform", "1989 - 1990 Uniform",
                "1979 - 1980 Uniform", "1972 - 1973 Uniform", "1971 - 1972 Uniform",
                "2003 Alternate 1", "2003 Alternate 2", "2004 Alternate 1",
                "2004 Alternate 2", "2004 Alternate 3"
            }},
            { "Eagles", new[] {
                "2003 - 2004 Uniform", "2000 - 2003 Uniform", "1996 - 1996 Uniform",
                "1993 - 1994 Uniform", "1989 - 1999 Uniform", "1979 - 1980 Uniform",
                "1969 - 1973 Uniform", "1967 - 1967 Uniform", "1958 - 1959 Uniform",
                "2000 Alternate 1", "2000 Alternate 2", "2003 Alternate 1",
                "2003 Alternate 2", "2004 Alternate 1", "2004 Alternate 2"
            }},
            { "Falcons", new[] {
                "2003 - 2004 Uniform", "1998 - 2003 Uniform", "1993 - 1994 Uniform",
                "1990 - 1997 Uniform", "1987 - 1989 Uniform", "1984 - 1986 Uniform",
                "1978 - 1979 Uniform", "1971 - 1977 Uniform", "1968 - 1969 Uniform",
                "1966 - 1967 Uniform", "2004 Alternate 1", "2004 Alternate 2",
                "2004 Alternate 3", "2004 Alternate 4"
            }},
            { "Giants", new[] {
                "2002 - 2004 Uniform", "2000 - 2002 Uniform", "1981 - 1999 Uniform",
                "1976 - 1980 Uniform", "1974 - 1975 Uniform", "1961 - 1974 Uniform",
                "1956 - 1960 Uniform", "2004 Alternate 1"
            }},
            { "Jaguars", new[] {
                "2002 - 2004 Uniform", "1995 - 2000 Uniform", "1996 - 1997 Uniform",
                "2003 Alternate 1", "2003 Alternate 2", "2003 Alternate 3", "2003 Alternate 4"
            }},
            { "Jets", new[] {
                "1998 - 2004 Uniform", "1993 - 1994 Uniform", "1990 - 1993 Uniform",
                "1978 - 1985 Uniform", "1965 - 1968 Uniform", "1963 - 1964 Uniform",
                "2003 Alternate 1"
            }},
            { "Lions", new[] {
                "2003 - 2004 Uniform", "2000 - 2003 Uniform", "1999 - 1999 Uniform",
                "1980 - 1993 Uniform", "1972 - 1976 Uniform", "1965 - 1968 Uniform",
                "1957 - 1965 Uniform", "2003 Alternate 1", "2004 Alternate 1", "2004 Alternate 2"
            }},
            { "Packers", new[] {
                "2000 - 2004 Uniform", "1993 - 1994 Uniform", "1983 - 1984 Uniform",
                "1966 - 1976 Uniform", "1952 - 1965 Uniform", "2003 Alternate 1", "2004 Alternate 1"
            }},
            { "Panthers", new[] {
                "1995 - 2004 Uniform", "1996 - 1996 Uniform", "2003 Alternate 1"
            }},
            { "Patriots", new[] {
                "2001 - 2004 Uniform", "1996 - 2000 Uniform", "1993 - 1994 Uniform",
                "1992 - 1993 Uniform", "1990 - 1992 Uniform", "1984 - 1987 Uniform",
                "1975 - 1981 Uniform", "1965 - 1969 Uniform", "1959 - 1960 Uniform",
                "2003 Alternate 1", "1994 Alternate 1", "2004 Alternate 1"
            }},
            { "Raiders", new[] {
                "2002 - 2004 Uniform", "1966 - 1967 Uniform", "1962 - 1963 Uniform",
                "1960 - 1962 Uniform", "2004 Alternate 1"
            }},
            { "Rams", new[] {
                "2003 - 2004 Uniform", "2002 - 2003 Uniform", "1998 - 1999 Uniform",
                "1993 - 1994 Uniform", "1978 - 1979 Uniform", "1965 - 1972 Uniform",
                "1957 - 1958 Uniform", "2000 Alternate 1", "2000 Alternate 2",
                "2003 Alternate 1", "2004 Alternate 1", "2004 Alternate 2", "2004 Alternate 3"
            }},
            { "Ravens", new[] {
                "2000 - 2004 Uniform", "1997 - 1999 Uniform", "1995 - 1996 Uniform", "2004 Alternate 1"
            }},
            { "Redskins", new[] {
                "2002 - 2004 Uniform", "1993 - 1994 Uniform", "1982 - 1982 Uniform",
                "1972 - 1977 Uniform", "1970 - 1971 Uniform", "1968 - 1969 Uniform",
                "1965 - 1967 Uniform", "1959 - 1964 Uniform", "2003 Alternate 1", "2004 Alternate 1"
            }},
            { "Saints", new[] {
                "2002 - 2004 Uniform", "2001 - 2002 Uniform", "1999 - 2000 Uniform",
                "1993 - 1994 Uniform", "1991 - 1992 Uniform", "1976 - 1982 Uniform",
                "1975 - 1976 Uniform", "1967 - 1975 Uniform", "2003 Alternate 1"
            }},
            { "Seahawks", new[] {
                "2002 - 2004 Uniform", "1983 - 2002 Uniform", "1976 - 1982 Uniform",
                "2003 Alternate 1", "2003 Alternate 2"
            }},
            { "Steelers", new[] {
                "2000 - 2004 Uniform", "1968 - 1976 Uniform", "1966 - 1967 Uniform",
                "1963 - 1965 Uniform", "1961 - 1962 Uniform", "1959 - 1960 Uniform"
            }},
            { "Texans", new[] {
                "2002 - 2004 Uniform", "2001 - 2002 Uniform", "2004 Alternate 1"
            }},
            { "Titans", new[] {
                "1999 - 2004 Uniform", "1989 - 1990 Uniform", "1974 - 1975 Uniform",
                "1972 - 1974 Uniform", "1967 - 1968 Uniform", "1960 - 1961 Uniform",
                "1999 Alternate 1", "2004 Alternate 1", "2004 Alternate 2"
            }},
            { "Vikings", new[] {
                "1998 - 2004 Uniform", "1993 - 1994 Uniform", "1980 - 1984 Uniform",
                "1972 - 1973 Uniform", "1961 - 1962 Uniform", "2004 Alternate 1", "2004 Alternate 2"
            }},
        };
    }
}
