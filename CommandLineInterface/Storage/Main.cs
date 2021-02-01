using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace CommandLineInterface
{
    public static class Storage
    {
        public static string XmlFileDirectory = @"C:\Users\Oli\Documents\Program Files\Cli-Discord\Storage.xml";

        private static Dictionary<string, string> sessionStorage
            = new Dictionary<string, string>() 
            {
                { "SomeSpreader", "This contains some args" },
                { "SomeOtherSpreader", "-this has \"some tags\""  }
            };

        private static Dictionary<string, string> sessionStorageTemp
            = new Dictionary<string, string>();

        private static Task DeserializerTask;

        public static string Fetch(string name)
        {
            DeserializerTask.Wait();
            string fetched = "";
            if (sessionStorage.TryGetValue(name, out fetched))
            {
                return fetched;
            }
            return "UNDEFINED_SPREADER";
        }

        public static void Store(string key, string value)
        {
            if (!sessionStorage.TryAdd(key, value))
            {
                sessionStorage[key] = value;
            }
        }

        // Initiates the DeserializerTask task 
        public static void Deserialize()
        {
            DeserializerTask = new Task(() =>
            {
                sessionStorage.Clear();
                XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));

                using (StreamReader reader = new StreamReader(XmlFileDirectory))
                {
                    List<Entry> list = (List<Entry>)serializer.Deserialize(reader);
                    foreach (Entry entry in list)
                    {
                        sessionStorage[entry.Key] = entry.Value;
                    }
                }
            });
            DeserializerTask.Start();
        }

        // Xml data stored indefinitely must must be serialized 
        public static void Serialize()
        {
            DeserializerTask.Wait();
            List<Entry> entries = new List<Entry>();
            foreach (string key in sessionStorage.Keys)
            {
                entries.Add(new Entry(key, sessionStorage[key]));
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));

            using (StreamWriter writer = File.CreateText(XmlFileDirectory))
            {
                serializer.Serialize(writer, entries);
            }
        }

        public class Entry
        {
            public string Key;
            public string Value;

            public Entry() { }

            public Entry(string key, string value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}
