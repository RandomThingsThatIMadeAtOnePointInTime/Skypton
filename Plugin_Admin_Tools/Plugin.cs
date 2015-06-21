using Skypton;
using PluginHandler;
using SKYPE4COMLib;

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Plugin_Admin_Tools
{
    public class Plugin : IPlugin
    {
        static readonly string pluginName = "Admin Tools";
        static readonly string pluginAuthor = "Scarsz";
        static readonly string pluginVersion = "1";
        static readonly string pluginDescription = "Provide admins with useful commands.";
        static readonly bool pluginAdminOnly = true;
        static readonly string[] pluginCommands = { "addadmin", "deladmin", "restart", "say", "setname", "setstatus" };

        public string Name { get { return pluginName; } }
        public string Author { get { return pluginAuthor; } }
        public string Version { get { return pluginVersion; } }
        public string Description { get { return pluginDescription; } }
        public bool AdminOnly { get { return pluginAdminOnly; } }
        public string[] Commands { get { return pluginCommands; } }

        public string Main(string command, string sender, Skype skype)
        {
            string arguments = command.Remove(0, command.Split(' ')[0].Length + 1);
            switch (command.Split(' ')[0])
            {
                case "addadmin":
                    return addAdmin(command.Split(' ')[1], sender);
                case "deladmin":
                    return delAdmin(command.Split(' ')[1], sender);
                case "restart":
                    restart();
                    break;
                case "say":
                    return say(arguments, skype);
                case "setname":
                    return setName(arguments, skype);
                case "setstatus":
                    return setStatus(arguments, skype);
            }
            return "An internal error has occured.";
        }

        static string addAdmin(string name, string sender)
        {
            if (sender != Skypton.Program.adminList[0])
                return "Only the owner of the bot can add admins.";
            if (Skypton.Program.checkIfAdmin(name))
                return "User is already an admin.";
            Skypton.Program.adminList.Add(name);
            return "User has had admin privileges granted.";
        }
        static string delAdmin(string name, string sender)
        {
            if (sender != Skypton.Program.adminList[0])
                return "Only the owner of the bot can remove admins.";
            if (name == Skypton.Program.adminList[0])
                return "You can't remove the owner of the bot from the admin list.";
            if (!Skypton.Program.checkIfAdmin(name))
                return "User is already not an admin.";
            Skypton.Program.adminList.Remove(name);
            return name + " has had their admin privileges revoked.";
        }
        static void restart()
        {
            System.Diagnostics.Process.Start(System.Windows.Forms.Application.ExecutablePath);
            System.Windows.Forms.Application.Exit();
        }
        static string say(string text, Skype skype)
        {
            foreach (User user in skype.Friends)
            {
                skype.SendMessage(user.Handle, text);
            }
            return "Sent message to " + skype.Friends.Count + " contacts.";
        }
        static string setName(string name, Skype skype)
        {
            string oldName = skype.CurrentUserProfile.FullName;
            try
            {
                skype.CurrentUserProfile.FullName = name;
                return "Name set to \"" + name + "\"";
            }
            catch
            {
                skype.CurrentUserProfile.FullName = oldName;
                return "An error occured. The name has not been changed.";
            }
        }
        static string setStatus(string status, Skype skype)
        {
            string oldStatus = skype.CurrentUserProfile.FullName;
            try
            {
                skype.CurrentUserProfile.MoodText = status;
                return "Status set to \"" + status + "\"";
            }
            catch
            {
                skype.CurrentUserProfile.MoodText = oldStatus;
                return "An error occured. The status has not been changed.";
            }
        }
    }
}