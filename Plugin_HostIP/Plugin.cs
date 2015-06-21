using PluginHandler;
using SKYPE4COMLib;

using System;
using System.Net;

namespace Plugin_HostIP
{
    public class Plugin : IPlugin
    {
        static readonly string pluginName = "HostIP";
        static readonly string pluginAuthor = "Scarsz";
        static readonly string pluginVersion = "1";
        static readonly string pluginDescription = "Returns the numeric IP for a host string.";
        static readonly bool pluginAdminOnly = false;
        static readonly string[] pluginCommands = { "hostip" };

        public string Name { get { return pluginName; } }
        public string Author { get { return pluginAuthor; } }
        public string Version { get { return pluginVersion; } }
        public string Description { get { return pluginDescription; } }
        public bool AdminOnly { get { return pluginAdminOnly; } }
        public string[] Commands { get { return pluginCommands; } }

        public string Main(string command, string sender, Skype skype)
        {
            return Dns.GetHostAddresses(command.Split(' ')[1])[0].ToString();
        }
    }
}