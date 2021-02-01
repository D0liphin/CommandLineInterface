using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommandLineInterface 
{
    static public partial class Parser
    {
        static private string[] SplitUpArgs(string text, Func<string, string> UnEscape)
        {
            MatchCollection matches = Re.GetArguments.Matches(text);
            int i;
            int matches_Count = matches.Count;
            string[] args = new string[matches_Count];
            for (i = 0; i < matches_Count; ++i)
            {
                args[i] = UnEscape(StringTools.TrimOne(matches[i].Value, '"'));
            }
            return args;
        }

        static public CommandDetails GetDetails(string name, string[] scopes, Func<string, string> UnEscape)
        {
            string[] args = null; 
            var tags = new Dictionary<string, string[]>();

            int i; string scope; Match match;
            string tagName;
            string[] tagArgs;
            for (i = 0; i < scopes.Length; ++i)
            {
                scope = scopes[i];
                match = Re.SeparateTagFromArguments.Match(scope);
                if (!match.Success) 
                {
                    args = SplitUpArgs(scope, UnEscape);
                    for (int ai = 0; ai < scope.Length; ++ai)
                    {
                        args[i] = args[i].Trim('\"');
                    }
                }
                else
                {
                    tagName = match.Groups[1].Value;
                    tagName = StringTools.Slice(tagName, (1, tagName.Length));
                    tagArgs = SplitUpArgs(match.Groups[2].Value.Trim(' '), UnEscape);
                    if (!tags.TryAdd(tagName, tagArgs))
                    {
                        tags[tagName] = tagArgs;
                    }
                }
            }

            return new CommandDetails(name, args ?? new string[0], tags);
        }
    }
}
