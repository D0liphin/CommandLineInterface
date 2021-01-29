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
                quotes = quotes ?? new char[] { '\'', '"', '`' };
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
                    text = ReplaceRange(text, callback( Slice(text, (range.Item1, range.Item2 + 1)) ), range);
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
        
            private static string Jumbled(int length, char[] chars)
            {
                StringBuilder sb = new StringBuilder("", length);
                Random random = new Random();
                for (int i = 0; i < length; i++)
                {
                    sb.Append(chars[random.Next(chars.Length)]);
                }
                return sb.ToString();
            }

            private static Func<string, string> UnescaperFactory(string[] escapeCodes, Dictionary<string, char> unescapeDictionary)
            {
                return escapedText =>
                {
                    if (escapedText.Length < 8) { return escapedText; };
                    int i;
                    for (i = 0; i < escapeCodes.Length; ++i)
                    {
                        string thisCode = escapeCodes[i];
                        //Console.WriteLine($"escaping code \"{thisCode}\"");
                        escapedText = escapedText.Replace(thisCode, unescapeDictionary[thisCode].ToString());
                    }
                    return escapedText;
                };
            }

            static private char[] charsToJumble = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_'
            };

            public static Func<string, string> EscaperFactory(char[] toEscape, out Func<string, string> unEscape, char escapeChar = '\\')
            {
                int i;

                int toEscape_Length = toEscape.Length;
                string[] generatedEscapeCodes = new string[toEscape_Length];
                int escapeCodeCount = 0;
                string escapeCode;
                while (escapeCodeCount < toEscape_Length)
                {
                    escapeCode = Jumbled(8, charsToJumble);
                    if (!generatedEscapeCodes.Contains(escapeCode))
                    {
                        generatedEscapeCodes[escapeCodeCount] = escapeCode;
                        ++escapeCodeCount;
                    }
                }

                Dictionary<char, string> lookupDictionary = new Dictionary<char, string>();
                Dictionary<string, char> reverseLookupDictionary = new Dictionary<string, char>();
                for (i = 0; i < toEscape_Length; ++i)
                {
                    lookupDictionary[toEscape[i]] = generatedEscapeCodes[i];
                    reverseLookupDictionary[generatedEscapeCodes[i]] = toEscape[i];
                }

                unEscape = UnescaperFactory(generatedEscapeCodes, reverseLookupDictionary);

                return textToEscape => 
                {
                    int i2;
                    bool escapeThis = false;
                    StringBuilder sb = new StringBuilder();
                    for (i2 = 0; i2 < textToEscape.Length; ++i2)
                    {
                        if (textToEscape[i2] == escapeChar)
                        {
                            escapeThis = true;
                            continue;
                        }
                        if (escapeThis)
                        {
                            sb.Append(lookupDictionary[textToEscape[i2]]);
                            escapeThis = false;
                            continue;
                        }
                        sb.Append(textToEscape[i2]);
                    }

                    return sb.ToString();
                };
            }
        
            public static string EscapeIfInString(string text, Func<string, string> Escaper)
            {
                List<(int, int)> stringRanges = StringRanges(text);
                return ReplaceRanges(text, stringRanges, portion => Escaper(portion));
            }
        }
    }
}

// 012345
// -[][]-
// (0, 0)
// 