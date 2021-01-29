using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace CommandLineInterface
{
    public static partial class Parser
    {
        public static class RegularExpressions
        {
            private static string _string_      = "\".*?[^\\\\]\"";
            private static string _variable_    = @"[\w_\.\/][\w_\.\-\/]*";
            private static string _tag_         = @"\-[\w_]+";
            private static string _commandName_ = @"@?[\w_]+";
            private static string _argument_    = $"({_string_}|{_variable_})";
            private static string _arguments_   = $"({_argument_}( +|$))";

            // Separates the command name from its arguments (tags, default arguments etc.)
            // USEFUL DATA:
            //  [1] The command name
            //  [2] The remaining part of the string
            public static Regex SeparateCommandNameFromArguments
                = new Regex($"^({_commandName_})(.*)$");

            // 'scope' referring to the tag that they are bound to. 
            // Alternatively, an argument can be bound to the default scope, meaning it has no preceeding tag.
            // USEFUL DATA:
            //  Match[] of all scopes
            public static Regex GetScopes
                = new Regex($"(^{_arguments_}+|{_tag_}( +{_arguments_})*)");

            // Separates the tag name from its positional arguments
            // USEFUL DATA:
            //  [1] The tag name
            //  [2] The tag's arguments
            public static Regex SeparateTagFromArguments
                = new Regex($"^({_tag_})( +{_arguments_}+)?");

            // Matches all arguments
            // USEFUL DATA:
            //  [1] Trimmed argument
            public static Regex GetArguments
                = new Regex($"(^| )({_argument_})( |$)");

            public static void PrintCompiledRegularExpressions()
            {
                Console.WriteLine(
                    "SeparateCommandNameFromArguments | " + $"^({_commandName_})(.*)$"                     + "\n" +
                    "GetScopes                        | " + $"(^{_arguments_}+|{_tag_}( +{_arguments_})*)" + "\n" +
                    "SeparateTagFromArguments         | " + $"^({_tag_})( +{_arguments_}+)?"               + "\n" +
                    "GetArguments                     | " + $"(^| )({_argument_})( |$)"                    + "\n"
                    );
            }

            // Returns ranges (`List<(int, int)>` types) to represent all matches that are not in any of the specified ranges
            public static string MatchIfNotInRanges(Regex rx, string text, List<(int, int)> ranges)
            {
                return null;
            }
        }
    }
}
