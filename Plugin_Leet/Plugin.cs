using PluginHandler;
using SKYPE4COMLib;

using System;
using System.Collections.Generic;

namespace Plugin_Leet
{
    public class Plugin : IPlugin
    {
        static readonly string pluginName = "Leet";
        static readonly string pluginAuthor = "Scarsz";
        static readonly string pluginVersion = "1";
        static readonly string pluginDescription = "\"Leetify\" a given phrase.";
        static readonly bool pluginAdminOnly = false;
        static readonly string[] pluginCommands = { "leetify" };

        public string Name { get { return pluginName; } }
        public string Author { get { return pluginAuthor; } }
        public string Version { get { return pluginVersion; } }
        public string Description { get { return pluginDescription; } }
        public bool AdminOnly { get { return pluginAdminOnly; } }
        public string[] Commands { get { return pluginCommands; } }

        public string Main(string command, string sender, Skype skype)
        {
            string leetify = command.Remove(0, command.Split(' ')[0].Length + 1);

            Dictionary<string, string> leeter = new Dictionary<string, string>();

            leeter.Add("4", "a");
            //leeter.Add(@"/\", "a");
            //leeter.Add("@", "a");
            //leeter.Add("^", "a");

            leeter.Add("13", "b");
            //leeter.Add("|3", "b");
            //leeter.Add("/3", "b");
            //leeter.Add("8", "b");

            leeter.Add("(", "c");
            //leeter.Add("<", "c");

            leeter.Add("|)", "d");
            //leeter.Add("|>", "d");

            leeter.Add("3", "e");

            leeter.Add("9", "g");

            leeter.Add("/-/", "h");
            //leeter.Add("[-]", "h");
            //leeter.Add("]-[", "h");
            //leeter.Add("|-|", "h");

            leeter.Add("!", "i");
            //leeter.Add("1", "i");

            leeter.Add("_/", "j");
            //leeter.Add("_|", "j");

            leeter.Add("|_", "l");

            leeter.Add(@"/\/\", "m");

            leeter.Add("0", "o");

            leeter.Add("|`", "r");

            leeter.Add("$", "s");
            //leeter.Add("5", "s");

            leeter.Add("7", "t");
            //leeter.Add("`|`", "t");

            leeter.Add(@"\_/", "u");

            leeter.Add(@"\/", "v");

            leeter.Add(@"\/\/", "w");

            leeter.Add("><", "x");
            //leeter.Add(")(", "x");

            leeter.Add("2", "z");

            foreach (KeyValuePair<string, string> leet in leeter)
                leetify = leetify.Replace(leet.Value, leet.Key);
            
            return leetify;
        }
    }
}