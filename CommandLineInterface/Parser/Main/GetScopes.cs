using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommandLineInterface
{
    static public partial class Parser
    {
        static private string GetArguments(string text, out string name)
        {
            Match match = Re.SeparateCommandNameFromArguments.Match(text);
            name = match.Groups[1].Value;
            return match.Groups[2].Value.Trim(' ');
        }
        static public string[] GetScopes(string text, out string name)
        {
            string remainder = GetArguments(text, out name);

            MatchCollection matches = Re.GetScopes.Matches(remainder);
            int matches_Count = matches.Count;
            string[] scopes = new string[matches_Count];
            int i;
            for (i = 0; i < matches_Count; ++i)
            {
                scopes[i] = matches[i].Value;
            }
            return scopes;
        }
    }
}
