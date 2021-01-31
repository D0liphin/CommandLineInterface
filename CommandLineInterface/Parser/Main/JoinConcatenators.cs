using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommandLineInterface
{
    static public partial class Parser
    {
        static public string JoinConcatenators(string text)
        {
            while (true)
            {
                MatchCollection concatenatorMatches = Re.ToBeConcatenated.Matches(text);
                if (concatenatorMatches.Count == 0) break;
                List <(int, int)> concatenatorRanges = Re.ConvertMatchesToRanges(concatenatorMatches);

                int i;
                string concatenated;
                int concatenatorMatches_Count = concatenatorMatches.Count;
                Match thisMatch;
                for (i = concatenatorMatches_Count - 1; i >= 0 ; --i)
                {
                    thisMatch = concatenatorMatches[i];
                    concatenated = thisMatch.Groups[1].Value + thisMatch.Groups[2].Value;
                    text = StringTools.ReplaceRange(text, concatenated, concatenatorRanges[i]);
                }
            }
            return text;
        }
    }
}
