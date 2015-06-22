using PluginHandler;
using SKYPE4COMLib;
using Skypton;

using System;

namespace Plugin_Ping
{
    public class Plugin : IPlugin
    {
        static readonly string pluginName = "Help";
        static readonly string pluginAuthor = "Scarsz";
        static readonly string pluginVersion = "1";
        static readonly string pluginDescription = "Displays all available commands from each plugin.";
        static readonly bool pluginAdminOnly = false;
        static readonly string[] pluginCommands = { "commands", "help", "plugins" };

        public string Name { get { return pluginName; } }
        public string Author { get { return pluginAuthor; } }
        public string Version { get { return pluginVersion; } }
        public string Description { get { return pluginDescription; } }
        public bool AdminOnly { get { return pluginAdminOnly; } }
        public string[] Commands { get { return pluginCommands; } }

        public string Main(string command, string sender, Skype skype)
        {
            foreach (IPlugin plugin in Skypton.Program.pluginDictionary.Values)
            {
                string response = "";
                foreach (string commandFromPlugin in plugin.Commands)
                {
                    response += commandFromPlugin + ", ";
                }
                response = response.Remove(response.Length - 2, 2);
                response += " - " + plugin.Description;
                skype.SendMessage(sender, response);
                Console.WriteLine("> " + response);
            }
            return String.Empty;
        }
    }
}