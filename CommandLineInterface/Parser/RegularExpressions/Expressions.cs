using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace CommandLineInterface
{
    public static partial class Parser
    {
        public static partial class Re
        {
            private static string _string_      = "\".*?\"";
            private static string _identifier_  = @"[\w_\-]+";
            private static string _variable_    = @"[\w_\.\/][\w_\.\-\/]*";
            private static string _tag_         = $"\\-{_identifier_}";
            private static string _command_     = $"@?{_identifier_}";
            private static string _spreader_    = $"@{_identifier_}";
            private static string _argument_    = $"({_string_}|{_variable_})";
            private static string _separator_   = @"( +(\+ +)?|$)";
            private static string _arguments_   = $"({_argument_}{_separator_})";
            private static string _scopeargs_   = $"( +{_arguments_}*{_argument_})";

            // Validates a command (where characters have been escaped)
            private static readonly string Validate_string = $"{_command_}{_scopeargs_}?( +{_tag_}{_scopeargs_}?)* *";
            public static Regex Validate
                = new Regex(Validate_string);

            // Separates the command name from its arguments (tags, default arguments etc.)
            // USEFUL DATA:
            //  [1] The command name
            //  [2] The remaining part of the string
            private readonly static string SeparateCommandNameFromArguments_string = $"^({_command_})(.*)$";
            public static Regex SeparateCommandNameFromArguments
                = new Regex(SeparateCommandNameFromArguments_string);

            // 'scope' referring to the tag that they are bound to. 
            // Alternatively, an argument can be bound to the default scope, meaning it has no preceeding tag.
            // USEFUL DATA:
            //  MatchCollection of all scopes
            private readonly static string GetScopes_string = $"(^{_arguments_}+|{_tag_}( +{_arguments_}*)?)";
            public static Regex GetScopes
                = new Regex(GetScopes_string);

            // Separates the tag name from its positional arguments
            // USEFUL DATA:
            //  [1] The tag name
            //  [2] The tag's arguments
            private readonly static string SeparateTagsFromArguments_string = $"^({_tag_})({_separator_}{_arguments_}+)?";
            public static Regex SeparateTagFromArguments
                = new Regex(SeparateTagsFromArguments_string);

            // Matches all arguments
            // USEFUL DATA:
            //  MatchCollection of arguments
            private readonly static string GetArguments_string = $"{_argument_}";
            public static Regex GetArguments
                = new Regex(GetArguments_string);

            // Matches all spreaders 
            // USEFUL DATA:
            //  MatchCollection of spreaders 
            private readonly static string GetSpreaders_string = $" {_spreader_}";
            public static Regex GetSpreaders
                = new Regex(GetSpreaders_string);

            // Matches all sections to be concatenated
            // USEFUL DATA:
            //  MatchCollection of two variables that are to be concatenated
            //  [1] The first operand
            //  [2] the second operand
            private readonly static string ToBeConcatenated_string = $"{_argument_} +\\+ +{_argument_}";
            public static Regex ToBeConcatenated
                = new Regex(ToBeConcatenated_string);

            public static void PrintCompiledRegularExpressions()
            {
                Console.WriteLine(
                    "SeparateCommandNameFromArguments | " + SeparateCommandNameFromArguments_string + "\n" +
                    "GetScopes                        | " + GetScopes_string                        + "\n" +
                    "SeparateTagFromArguments         | " + SeparateTagsFromArguments_string        + "\n" +
                    "GetArguments                     | " + GetArguments_string                     + "\n" + 
                    "GetSpreaders                     | " + GetSpreaders_string                     + "\n" +
                    "ToBeConcatenated                 | " + ToBeConcatenated_string                 + "\n" +
                    "Validate                         | " + Validate_string                         + "\n"
                    );
            }
        }
    }
}
