using System;
using System.Text;
using System.Collections.Generic;

namespace CommandLineInterface
{
    public class CommandDetails 
    {
        public readonly string Name;
        public readonly string[] Args;
        public readonly Dictionary<string, string[]> Tags;
        public readonly bool BadFormat;

        public CommandDetails(string name, string[] args, Dictionary<string, string[]> tags, bool badFormat=false)
        {
            this.Name = name;
            this.Args = args;
            this.Tags = tags;
            this.BadFormat = badFormat;
        }

        private string StringifyArgs(string[] args, int indent=1)
        {
            string stringified = "";
            if (args.Length == 0) return "[]";
            foreach (string arg in args) stringified += $"{new string(' ', 4*indent)}\"{arg}\",\n";
            return  "[\n" + stringified + new string(' ', 4 * (indent - 1)) + "],";
        }

        private string StringifyTags()
        {
            string stringified = "";
            if (Tags.Keys.Count == 0) return "{}";
            foreach (string tag in Tags.Keys) 
                stringified += $"    \"{tag}\": {StringifyArgs(Tags[tag], 2)}\n";
            return "{\n" + stringified + "}";
        }

        public override string ToString()
        {
            return
            Name + " " +
            StringifyArgs(Args) + "\n" +
            StringifyTags() + "\n"
            ;
        }
    }
}