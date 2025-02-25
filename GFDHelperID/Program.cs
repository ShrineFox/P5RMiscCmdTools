using GFDLibrary;
using GFDLibrary.Common;
using GFDStudio.FormatModules;
using YamlDotNet.Core.Tokens;

namespace GFDHelperID
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inputModels = Directory.GetFiles(args[0], "*.GMD", SearchOption.AllDirectories);

            foreach (var gmd in inputModels)
            {
                Console.WriteLine(gmd);

                var modelPack = ModuleImportUtilities.ImportFile<ModelPack>(gmd);
                
                foreach (var node in modelPack.Model.Nodes)
                {
                    // Remove scaling/mapchannel property
                    foreach (var prop in node.Properties)
                    {
                        if (prop.Key == "ScalingMax" || prop.Key == "MapChannel:8")
                            node.Properties.Remove(prop);
                    }

                    // Add helper IDs for attaching item models
                    if (node.Name == "root")
                    {
                        node.Properties.Add(new UserIntProperty("gfdHelperID", 420));
                    }
                    if (node.Name == "rot")
                    {
                        node.Properties.Add(new UserIntProperty("gfdHelperID", 69));
                    }

                    // Log helper IDs
                    foreach (UserProperty prop in node.Properties.Values)
                    {
                        string logString = $"\t{node.Name}: {prop.ToUserPropertyString()}";
                        Console.WriteLine(logString);
                    }
                }

                modelPack.Save(gmd);
            }

            Console.WriteLine($"Done.");
            Console.ReadKey();
        }
    }
}
