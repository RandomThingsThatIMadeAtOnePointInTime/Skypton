using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INIHandler;
using PluginHandler;
using SKYPE4COMLib;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Skypton
{
    public class Program
    {
        // Config stuff, don't touch
        public static string buildId = "1";
        public static string build;
        public static string pluginsFolder;
        public static string trigger;
        public static List<string> adminList = new List<string>();
        public static bool loggingEnabled;
        public static string logFile;

        // Plugin loader information, don't touch
        public static Dictionary<string, IPlugin> pluginDictionary = new Dictionary<string, IPlugin>();
        public static Dictionary<string[], IPlugin> pluginCommandDictionary = new Dictionary<string[], IPlugin>();
        public static ICollection<IPlugin> plugins;
        // Skype information, don't touch
        public static Skype skype = new Skype();
        // INI handler, don't touch
        public static IniHandler ini = new IniHandler(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "") + "\\Skypton.ini");
        // Queues, don't touch
        public static List<ChatMessage> commandQueue = new List<ChatMessage>();
        
        static void Main(string[] args)
        {
            init();
            skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(MessageReceived);
            Thread CommandProcessorThread = new Thread(new ThreadStart(CommandProcessor));
            CommandProcessorThread.Start();
        }

        static void CommandProcessor()
        {
            while (true)
            {
                if (commandQueue.Count > 0)
                {
                    string command = commandQueue[0].Body.Remove(0, trigger.Length).ToLower(); //!URBAN CykA -> urban cyka
                    string sender = commandQueue[0].Sender.Handle;
                    ProcessCommand(command, sender);
                    commandQueue.RemoveAt(0);
                }
                else { Thread.Sleep(100); }
            }
        }
        static void ProcessCommand(string command, string sender)
        {
            // Inform about processing command
            writeInfo(String.Format("Processor: processing command received from \"{0}\": {1}", sender, command), "process");
            
            // Assign string to plugin that can process the command
            IPlugin plugin = null;

            // Find the first plugin in order from first to last loaded that can process this command
            foreach (var pluginItem in pluginCommandDictionary)
                foreach (string commandFromArray in pluginItem.Key)
                    if (commandFromArray == command.Split(' ')[0])
                    {
                        plugin = pluginItem.Value;
                        break;
                    }

            // String to store result that will be sent back
            string result;

            // Check to make sure we have a plugin able to process the command, if we have a plugin for it process the command, else unknown
            if (plugin != null) 
            {
                // Check to see if the plugin is admin only, if so don't run it if not admin
                if (plugin.AdminOnly)
                    if (checkIfAdmin(sender))
                        result = runPlugin(command, sender, plugin);
                    else
                    {
                        result = "Command not found: " + command;
                        writeInfo(sender + " tried to issue command \"" + command + "\" which is an admin-only command!", "severe");
                    }
                else
                    result = runPlugin(command, sender, plugin);
            }
            else { result = "Command not found: " + command; }

            // Write result to console
            writeInfo(String.Format("-> {0}", result), "process");

            // Send result
            if (sender != skype.CurrentUserHandle)
                skype.SendMessage(sender, result);

            // Write white line in console to show breaks between messages
            ConsoleColor oldColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i < Console.BufferWidth; i++)
                Console.Write(" ");
            Console.BackgroundColor = oldColor;
        }
        static void MessageReceived(ChatMessage msg, TChatMessageStatus status)
        {
            // Don't fire for read messages
            if (msg.Status == TChatMessageStatus.cmsRead)
                return;
            // Don't do anything unless the received message has the trigger
            if (msg.Body.IndexOf(trigger) != 0)
                return;
            // Don't add the message to the queue if it's already there
            if (commandQueue.Contains(msg))
                return;

            // Inform about received command
            writeInfo(String.Format("Skype: received command from \"{0}\": {1}", msg.Sender.Handle, msg.Body.Remove(0, trigger.Length).ToLower()), "receive");

            commandQueue.Add(msg);
        }

        static void init()
        {
            loadConfig();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Skypton [{0}]", build);
            Console.Title = "Skypton [" + build + "]";
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Initializing plugins... ");
            loadPlugins();
            Console.WriteLine("loaded {0} plugins", plugins.Count);
            Console.Write("Attaching to Skype... ");
            skype.Attach(7);
            Console.WriteLine("done");
            Console.WriteLine("Initialization done\n");
            listPlugins();
        }
        static void loadConfig()
        {
            // Build ID ("#")
            if (ini.IniReadValue("Build") == String.Empty || !Regex.IsMatch(ini.IniReadValue("Build"), @"^\d+$"))
                ini.IniWriteValue("Build", buildId);
            build = "Build " + ini.IniReadValue("Build");

            // Plugins folder ("Plugins")
            if (ini.IniReadValue("PluginsFolder") == String.Empty)
                ini.IniWriteValue("PluginsFolder", "Plugins");
            pluginsFolder = ini.IniReadValue("PluginsFolder");

            // Trigger ("!")
            if (ini.IniReadValue("Trigger") == String.Empty)
                ini.IniWriteValue("Trigger", "!");
            trigger = ini.IniReadValue("Trigger");

            // Admins ("somebody1,somebody2,somebody3")
            if (ini.IniReadValue("Admins") == String.Empty)
                ini.IniWriteValue("Admins", "");
            foreach (string admin in ini.IniReadValue("Admins").Split(','))
                adminList.Add(admin);

            // Logging (true, "Skypton.log")
            if (ini.IniReadValue("LoggingEnabled") == String.Empty)
                ini.IniWriteValue("LoggingEnabled", "true");
            try { loggingEnabled = Convert.ToBoolean(ini.IniReadValue("LoggingEnabled")); } catch { loggingEnabled = true; }
            if (ini.IniReadValue("LogFile") == String.Empty)
                ini.IniWriteValue("LogFile", "Skypton.log");
            logFile = ini.IniReadValue("LogFile");
        }
        static void loadPlugins()
        {
            plugins = PluginLoader<IPlugin>.LoadPlugins(pluginsFolder);

            if (plugins == null)
                plugins = new List<IPlugin>();

            foreach (var item in plugins)
            {
                if (!pluginDictionary.ContainsKey(item.Name))
                    pluginDictionary.Add(item.Name, item);
                if (!pluginCommandDictionary.ContainsKey(item.Commands))
                    pluginCommandDictionary.Add(item.Commands, item);
            }
        }
        static void listPlugins()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(">");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Plugins");

            foreach (var plugin in pluginDictionary)
            {
                if (plugin.Value.AdminOnly)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(plugin.Value.Name);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" v" + plugin.Value.Version);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(" - ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(plugin.Value.Description);
            }
            Console.WriteLine();
        }
        static string runPlugin(string command, string sender, IPlugin plugin)
        {
            return plugin.Main(command, sender, skype);
        }

        public static bool checkIfAdmin(string name)
        {
            return adminList.Contains(name);
        }
        static void writeInfo(string info, string reporter)
        {
            if (reporter == "receive") { Console.ForegroundColor = ConsoleColor.Green; }
            if (reporter == "process") { Console.ForegroundColor = ConsoleColor.Magenta; }
            if (reporter == "severe") { Console.ForegroundColor = ConsoleColor.Red; }

            // Show to console
            Console.WriteLine("[{0}] {1}", DateTime.Now, info);

            // Write to log
            if (loggingEnabled)
                using (StreamWriter sw = File.AppendText(logFile))
                    sw.WriteLine(String.Format("[{0}] {1}", DateTime.Now, info));
        }
    }
}