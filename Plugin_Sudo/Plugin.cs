using PluginHandler;
using SKYPE4COMLib;

using System;

namespace Plugin_Sudo
{
    public class Plugin : IPlugin
    {
        static readonly string pluginName = "Sudo";
        static readonly string pluginAuthor = "Scarsz";
        static readonly string pluginVersion = "1";
        static readonly string pluginDescription = "Make the bot say a message back to you.";
        static readonly bool pluginAdminOnly = false;
        static readonly string[] pluginCommands = { "sudo" };

        public string Name { get { return pluginName; } }
        public string Author { get { return pluginAuthor; } }
        public string Version { get { return pluginVersion; } }
        public string Description { get { return pluginDescription; } }
        public bool AdminOnly { get { return pluginAdminOnly; } }
        public string[] Commands { get { return pluginCommands; } }

        public string Main(string command, string sender, Skype skype)
        {
            if (command.Remove(0, command.Split(' ')[0].Length) == "")
                return "You can't just expect me to pull words out of my ass, here. Give me something to say.";

            return command.Remove(0, command.Split(' ')[0].Length);
        }
    }
}