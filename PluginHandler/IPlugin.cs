using SKYPE4COMLib;

namespace PluginHandler
{
    public interface IPlugin
    {
        string Name { get; }
        string Author { get; }
        string Version { get; }
        string Description { get; }
        string[] Commands { get; }
        string Main(string command, Skype skype);
    }
}