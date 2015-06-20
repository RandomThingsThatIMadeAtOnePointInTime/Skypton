using PluginHandler;
using SKYPE4COMLib;

using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace Plugin_Ping
{
    public class Plugin : IPlugin
    {
        static readonly string pluginName = "Urban Dictionary";
        static readonly string pluginAuthor = "Scarsz";
        static readonly string pluginVersion = "1";
        static readonly string pluginDescription = "Gets the top definition of an Urban Dictionary phrase";
        static readonly string[] pluginCommands = { "urban" };

        public string Name { get { return pluginName; } }
        public string Author { get { return pluginAuthor; } }
        public string Version { get { return pluginVersion; } }
        public string Description { get { return pluginDescription; } }
        public string[] Commands { get { return pluginCommands; } }

        public string Main(string command, Skype skype)
        {
            string result = "No definition found";

            string term = command.Remove(0, command.Split(' ')[0].Length + 1).Replace(" ", "+"); // bye+felicia
            string[] parse;
            using (WebClient wc = new WebClient())
                parse = wc.DownloadString("http://www.urbandictionary.com/define.php?term=" + term).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in parse)
                if (line.Contains("class"))
                    if (line.Contains("meaning"))
                    {
                        result = parse[Array.IndexOf(parse, line) + 1];
                        break;
                    }
            return HttpUtility.HtmlDecode(result).Replace("<br>", "").Replace("<br/>", "");
        }
    }
}