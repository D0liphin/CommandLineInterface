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
            Func<string, string> UnEscape;
            Func<string, string> Escape = StringTools.EscaperFactory(toEscape, out UnEscape);
            input = StringTools.EscapeIfInString(input, Escape);
            input = ReplaceSpreaders(input);
            input = JoinConcatenators(input);
            string name;
            string[] scopes = GetScopes(input, out name);
            CommandDetails details = GetDetails(name, scopes, UnEscape);

            // if storage
            if (command[0] == '@')
            {
                Match match = Re.SeparateCommandNameFromArguments.Match(command);
                string value = "\"" + String.Join(' ', details.Args) + "\" ";
                foreach (string tagKey in details.Tags.Keys)
                {
                    value += "-" + tagKey + " \"" + String.Join(' ', details.Tags[tagKey]) + "\"";
                }
                Storage.Store(name.Trim('@'), value);
            }
            return details;
        }
    }
}
