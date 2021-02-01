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
            public static string Slice(string text, (int, int) range)
            {
                return text.Substring(range.Item1, range.Item2 - range.Item1);
            }

            public static string TrimOne(string text, char character)
            {
                if (text[0] == character) text = Slice(text, (1, text.Length));
                if (text[text.Length - 1] == character) text = Slice(text, (0, text.Length - 1));
                return text;
            }
        }
    }
}