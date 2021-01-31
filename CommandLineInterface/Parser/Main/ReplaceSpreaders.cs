using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommandLineInterface
{
    static public partial class Parser
    {
        static public string ReplaceSpreaders(string input, List<(int, int)> stringRanges=null)
        {
            stringRanges = stringRanges ?? StringTools.StringRanges(input);

            List<Match> spreaderMatches = Re.MatchIfNotInRanges(Re.GetSpreaders.Matches(input), stringRanges);
            List<(int, int)> spreaderRanges = Re.ConvertMatchesToRanges(spreaderMatches);
            return StringTools.ReplaceRanges(input, spreaderRanges, text => 
                " " + Storage.Fetch(text.Trim(' ').Trim('@'))
                );
        }
    }
}
