using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using static CommandLineInterface.Parser.StringTools;

namespace CommandLineInterface
{
    public static partial class Parser
    {
        public static partial class RegularExpressions
        {
            // Coverts a MatchCollection to a List<(int, int)>
            public static List<(int, int)> ConvertMatchesToRanges(MatchCollection matches)
            {
                List<(int, int)> ranges = new List<(int, int)>();
                int i; Match thisMatch;
                int matches_Count = matches.Count;
                for (i = 0; i < matches_Count; ++i)
                {
                    thisMatch = matches[i];
                    ranges.Add((thisMatch.Index, thisMatch.Length + thisMatch.Index - 1));
                }
                return ranges;
            }

            // Coverts a MatchCollection to a List<(int, int)>
            public static List<(int, int)> ConvertMatchesToRanges(List<Match> matches)
            {
                List<(int, int)> ranges = new List<(int, int)>();
                int i; Match thisMatch;
                int matches_Count = matches.Count;
                for (i = 0; i < matches_Count; ++i)
                {
                    thisMatch = matches[i];
                    ranges.Add((thisMatch.Index, thisMatch.Length + thisMatch.Index - 1));
                }
                return ranges;
            }

            // Returns ranges (`List<(int, int)>` types) to represent all matches that are not
            // even partially in any of the specified ranges
            public static List<Match> MatchIfNotInRanges(MatchCollection matches, List<(int, int)> ranges)
            {
                int i; Match thisMatch;
                int ri; (int, int) thisRange;
                int matches_Count = matches.Count;
                int ranges_Count = ranges.Count;
                bool isGoodMatch;
                List<Match> matchesNotInRanges = new List<Match>();

                for (i = 0; i < matches_Count; ++i)
                {
                    thisMatch = matches[i];
                    isGoodMatch = true;
                    for (ri = 0; ri < ranges_Count; ++ri)
                    {
                        thisRange = ranges[ri];
                        if (!(
                            !InRange(thisMatch.Index, thisRange) &&
                            !InRange(thisMatch.Index + thisMatch.Length - 1, thisRange)
                            ))
                        {
                            isGoodMatch = false;
                            break;
                        }
                    }
                    if (isGoodMatch)
                    {
                        matchesNotInRanges.Add(thisMatch);
                    }
                }

                return matchesNotInRanges;
            }
        }
    }
}
