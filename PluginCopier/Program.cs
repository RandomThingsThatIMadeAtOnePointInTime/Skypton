using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PluginCopier
{
    class Program
    {
        static void Main(string[] args)
        {
            string destination = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Skypton\bin\Debug");
            Directory.SetCurrentDirectory(destination);

            List<string> files = new List<string>();
            foreach (string file in Directory.EnumerateFiles(@"..\..\..", "Plugin_*.dll", SearchOption.AllDirectories))
                if (!files.Contains(file.Split('\\').Last()))
                    if (file.Contains("bin"))
                        if (!file.Contains("Plugins"))
                            files.Add(file);
            
            Console.WriteLine("\nCopying plugins to build folder...");
            if (Directory.Exists("Plugins"))
                foreach (string plugin in Directory.EnumerateFiles("Plugins", "Plugin_*.dll", SearchOption.AllDirectories))
                    File.Delete(plugin);
            else
                Directory.CreateDirectory("Plugins");
            foreach (string plugin in files)
            {
                Console.WriteLine("Coping " + plugin + " -> Plugins\\" + plugin.Split('\\').Last());
                File.Copy(plugin, destination + "\\Plugins\\" + plugin.Split('\\').Last(), true);
            }
            Console.WriteLine();
        }
    }
}
