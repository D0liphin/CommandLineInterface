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

        public CommandDetails(string name, string[] args, Dictionary<string, string[]> tags)
        {
            this.Name = name;
            this.Args = args;
            this.Tags = tags;
        }

        private string StringifyArgs(string[] args)
        {
            string stringified = "";
            foreach (string arg in args) stringified += $"    {arg},\n";
            return "[\n" + stringified + "]";
        }

        private string StringifyTags()
        {
            string stringified = "";
            foreach (string tag in Tags.Keys) 
                stringified += $"\"{tag}\": {StringifyArgs(Tags[tag])}\n";
            return stringified;
        }

        public override string ToString()
        {
            return
                Name + "\n" +
                StringifyArgs(Args) + "\n" +
                StringifyTags() + "\n"
                ;
        }
    }
}