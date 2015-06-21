using PluginHandler;
using SKYPE4COMLib;

using System;

namespace Plugin_Random
{
    public class Plugin : IPlugin
    {
        static readonly string pluginName = "Random";
        static readonly string pluginAuthor = "Scarsz";
        static readonly string pluginVersion = "1";
        static readonly string pluginDescription = "Generates a number between the two given numbers (inclusive)";
        static readonly bool pluginAdminOnly = false;
        static readonly string[] pluginCommands = { "random" };

        public string Name { get { return pluginName; } }
        public string Author { get { return pluginAuthor; } }
        public string Version { get { return pluginVersion; } }
        public string Description { get { return pluginDescription; } }
        public bool AdminOnly { get { return pluginAdminOnly; } }
        public string[] Commands { get { return pluginCommands; } }

        public string Main(string command, string sender, Skype skype)
        {
            int number1;
            if (!int.TryParse(command.Split(' ')[1], out number1))
                return "Number 1 isn't a valid integer!";
            int number2;
            if (!int.TryParse(command.Split(' ')[2], out number2))
                return "Number 2 isn't a valid integer!";
            return new Random().Next(number1, number2 + 1).ToString();
        }
    }
}