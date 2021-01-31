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
        public static partial class StringTools
        {
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
                        if (escapeThis)
                        {
                            sb.Append(lookupDictionary[textToEscape[i2]]);
                            escapeThis = false;
                            continue;
                        }
                        if (textToEscape[i2] == escapeChar)
                        {
                            escapeThis = true;
                            continue;
                        }
                        sb.Append(textToEscape[i2]);
                    }

                    return sb.ToString();
                };
            }

            public static string EscapeIfInString(string text, Func<string, string> Escaper, List<(int, int)> stringRanges=null)
            {
                stringRanges = stringRanges ?? StringRanges(text);
                return ReplaceRanges(text, stringRanges, portion => Escaper(portion));
            }
        }
    }
}