using PluginHandler;
using SKYPE4COMLib;

using System;
using System.Linq;

namespace Plugin_Flip
{
    public class Plugin : IPlugin
    {
        static readonly string pluginName = "Flip";
        static readonly string pluginAuthor = "Scarsz";
        static readonly string pluginVersion = "1";
        static readonly string pluginDescription = "Flip the given text upside down.";
        static readonly bool pluginAdminOnly = false;
        static readonly string[] pluginCommands = { "flip" };

        public string Name { get { return pluginName; } }
        public string Author { get { return pluginAuthor; } }
        public string Version { get { return pluginVersion; } }
        public string Description { get { return pluginDescription; } }
        public bool AdminOnly { get { return pluginAdminOnly; } }
        public string[] Commands { get { return pluginCommands; } }

        public string Main(string command, string sender, Skype skype)
        {
            string input = command.Remove(0, command.Split(' ')[0].Length + 1);

            char[] X = @"¿/˙'\‾¡zʎxʍʌnʇsɹbdouɯlʞɾıɥƃɟǝpɔqɐ".ToCharArray();
            string V = @"?\.,/_!zyxwvutsrqponmlkjihgfedcba";

            return new string((from char obj in input.ToCharArray()
                               select (V.IndexOf(obj) != -1) ? X[V.IndexOf(obj)] : obj).Reverse().ToArray());
        }
    }
}