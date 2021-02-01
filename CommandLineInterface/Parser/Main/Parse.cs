using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace CommandLineInterface
{
    static public partial class Parser
    {
        static private char[] toEscape = new char[] { '\'', '"', '\\' };

        static public CommandDetails Parse(string command)
        {
            string input = command;

            // if storage
            if (command[0] == '@')
            {
                Match match = Re.SeparateCommandNameFromArguments.Match(command);
                string toStore = match.Groups[1].Value;
                Storage.Store(match.Groups[1].Value.Trim('@'), toStore);
                return null;
            }
                
            Func<string, string> UnEscape;
            Func<string, string> Escape = StringTools.EscaperFactory(toEscape, out UnEscape);
            input = StringTools.EscapeIfInString(input, Escape);
            input = ReplaceSpreaders(input);
            input = JoinConcatenators(input);
            string name;
            string[] scopes = GetScopes(input, out name);
            return GetDetails(name, scopes, UnEscape);
        }
    }
}
