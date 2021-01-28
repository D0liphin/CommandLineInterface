using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommandLineInterface
{
    public static partial class Parser
    {
        /// <summary>
        /// Includes various tools for handling strings
        /// Also includes tools for handling List<(int, int)> (ranges)
        /// </summary>
        public static class StringTools
        {
            public static List<(int, int)> StringRanges(string text, char[] quotes = null, char escapeChar = '\\')
            {
                quotes = quotes ?? new char[] { '\'', '"' };
                int charIndex; 
                int maxIndex = text.Length;
                char thisChar;
                bool escapeNext = false;
                bool inString = false;
                char stringStartChar = ' ';
                int rangeStartIndex = 0;
                List<(int, int)> ranges = new List<(int, int)>();

                for (charIndex = 0; charIndex < maxIndex; ++charIndex)
                {
                    if (escapeNext)
                    {
                        escapeNext = false;
                        continue;
                    }

                    thisChar = text[charIndex];
                    
                    if (thisChar == escapeChar)
                    {
                        escapeNext = true;
                        continue;
                    }

                    if (!inString)
                    {
                        if (quotes.Contains(thisChar))
                        {
                            inString = true;
                            stringStartChar = thisChar;
                            rangeStartIndex = charIndex;
                        }
                    } 
                    else 
                    {
                        if (thisChar == stringStartChar)
                        {
                            inString = false;
                            ranges.Add((rangeStartIndex, charIndex));
                        }
                    }
                }

                return ranges;
            }

            public static string Slice(string text, (int, int) range)
            {
                return text.Substring(range.Item1, range.Item2 - range.Item1);
            }

            public static string ReplaceRange(string text, string replacement, (int, int) range)
            {
                return Slice(text, (0, range.Item1)) + replacement + Slice(text, (range.Item2 + 1, text.Length));
            }

            /// <summary>
            /// Replaces all ranges 
            /// </summary>
            /// <param name="text">The text that will have sections replaced</param>
            /// <param name="ranges">The ranges to replace</param>
            /// <param name="callback">A callback taking the string that the range grabbed as a parameter and returning what it should replace that section with</param>
            /// <returns>string</returns>
            public static string ReplaceRanges(string text, List<(int, int)> ranges, Func<string, string> callback)
            {
                ranges.Reverse();
                foreach (var range in ranges)
                {
                    text = ReplaceRange(text, callback(Slice(text, range)), range);
                }
                return text;
            }

            /// <summary>
            /// gets ranges (within a limit) that aren't represented by `ranges`  
            /// input  |   [  ]   []  [  ][  ]  []    |  
            /// output |[ ]    [ ]  []        []  [  ]|   
            /// </summary>
            /// <param name="ranges"></param>
            /// <param name="finalIndex">The index that represents the final character of the entire range</param>
            /// <returns>List<(int, int)></returns>
            public static List<(int, int)> GetGapRanges(List<(int, int)> ranges, int finalIndex)
            {
                var gapRanges = new List<(int, int)>();
                int ranges_Count = ranges.Count; int i;

                if (ranges[0].Item1 > 0)
                {
                    gapRanges.Add((0, ranges[0].Item1 - 1));
                    
                }
                int lastIndex = ranges[0].Item2;

                for (i = 1; i < ranges_Count; ++i)
                {
                    var range = ranges[i];
                    if (lastIndex + 1 != range.Item1) 
                    {
                        gapRanges.Add((lastIndex + 1, range.Item1 - 1));
                    } // in cases where ranges are next to eachother
                    lastIndex = range.Item2;
                }

                if (finalIndex > lastIndex)
                {
                    gapRanges.Add((lastIndex + 1, finalIndex));
                }

                return gapRanges;
            }
        }
    }
}

// 012345
// -[][]-
// (0, 0)
// 