using SKYPE4COMLib;

namespace PluginHandler
{
    public interface IPlugin
    {
        string Name { get; }
        string Author { get; }
        string Version { get; }
        string Description { get; }
        bool AdminOnly { get; }
        string[] Commands { get; }
        string Main(string command, Skype skype);
    }
}