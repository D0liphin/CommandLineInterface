using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace CommandLineInterface
{
    static public partial class Parser
    {
        static private char[] toEscape = new char[] { '\'', '"', '\\' };

        static private string SerializeArguments(string[] args)
        {
            string serialized = "";
            int i;
            for (i = 0; i < args.Length; ++i)
            {
                serialized += " \"" + args[i] + "\"";
            }
            return serialized;
        }

        static private string Serialize(CommandDetails details)
        {
            string serialized = "";
            int i;
            var keys = details.Tags.Keys;
            foreach (string key in keys)
            {
                serialized += " -" + key + " " + SerializeArguments(details.Tags[key]);
            }
            return SerializeArguments(details.Args) + serialized;
        }

        static public CommandDetails Parse(string command)
        {
            string input = command;                
            Func<string, string> UnEscape;
            Func<string, string> Escape = StringTools.EscaperFactory(toEscape, out UnEscape);
            input = StringTools.EscapeIfInString(input, Escape);
            input = ReplaceSpreaders(input);

            if (!Re.Validate.IsMatch(input)) 
                return new CommandDetails("[PARSING ERROR] Invalid format.", new string[0], new Dictionary<string, string[]>(), true);

            input = JoinConcatenators(input);
            string name;
            string[] scopes = GetScopes(input, out name);
            CommandDetails details = GetDetails(name, scopes, UnEscape);

            // if storage
            if (command[0] == '@')
                Storage.Store(name.Trim('@'), Serialize(details));
            
            return details;
        }
    }
}
