using System;
using System.Text;
using System.Collections.Generic;

namespace CommandLineInterface
{
    static public partial class Parser
    {
        static public CommandDetails Parse(string command)
        {
            string input = command;
            // Console.WriteLine("[INPUT]                  " + input);

            // (1) escape strings
            Func<string, string> UnEscape;
            Func<string, string> Escape = Parser.StringTools.EscaperFactory(new char[] { '\'', '"', '\\' }, out UnEscape);
            input = Parser.StringTools.EscapeIfInString(input, Escape);
            // Console.WriteLine("[ESCAPE SPECIAL CHARS]   " + input);

            // (2) expand spreaders
            input = Parser.ReplaceSpreaders(input);
            // Console.WriteLine("[EXPAND SPREADERS]       " + input);

            // (3) join concatenators
            input = Parser.JoinConcatenators(input);
            // Console.WriteLine("[JOIN CONCATENATORS]     " + input);

            // (4) Split command name from args
            // (5) Get Scopes
            string name;
            string[] scopes = Parser.GetScopes(input, out name);
            // Console.WriteLine("\n[SPLIT NAME FROM ARGS]   " + name);
            // Console.WriteLine("\n[GET SCOPES]");
            // foreach (string scope in scopes) Console.WriteLine("\t" + scope);

            // (3) split tag from args
            // (4) split up args 
            return Parser.GetDetails(name, scopes, UnEscape);
            // Console.WriteLine(details);
        }
    }
}
