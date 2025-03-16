using GFDLibrary;
using GFDLibrary.Common;
using GFDStudio.FormatModules;

namespace GFDMatDumper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DumpMatsFromGMDs(args[0]);
            //CombineYAMLIntoDump(args[0]);

            Console.WriteLine($"Done.");
            Console.ReadKey();
        }

        private static void CombineYAMLIntoDump(string dirPath)
        {
            List<string> dumpLines = new List<string>();
            foreach (var yaml in Directory.GetFiles(dirPath, "*.yaml", SearchOption.AllDirectories))
            {
                foreach (var line in File.ReadAllLines(yaml).Where(x => x.StartsWith("#") || x.StartsWith("Flags:") || x.StartsWith("  GeometryFlags:") || x.StartsWith("  VertexAttributeFlags:")))
                    dumpLines.Add(line);
                dumpLines.Add("\r\n");
            }
            File.WriteAllLines("output.txt", dumpLines);
        }

        private static void DumpMatsFromGMDs(string dirPath)
        {
            var inputModels = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories).Where(x => x.ToLower().EndsWith(".gmd") || x.ToLower().EndsWith(".gfs"));
            string outDir = "./Materials";
            Directory.CreateDirectory(outDir);
            List<string> lines = new List<string>();

            foreach (var gmd in inputModels)
            {
                Console.WriteLine(gmd);

                try
                {
                    var modelPack = ModuleImportUtilities.ImportFile<ModelPack>(gmd);

                    if (modelPack.Materials != null && modelPack.Materials.Count > 0)
                    {
                        Console.WriteLine(gmd);
                        foreach (var mat in modelPack.Materials.Materials)
                        {
                            string outPath = Path.Combine(Path.GetFullPath(outDir), mat.Name.Replace(":", "_").Replace("\\", "_") + ".yaml");
                            if (File.Exists(outPath))
                            {
                                outPath = Path.Combine(Path.GetFullPath(outDir + $"\\{Path.GetFileName(gmd)}"), mat.Name.Replace(":", "_").Replace("\\", "_") + ".yaml");
                                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                            }
                            mat.SaveYamlFile(outPath);
                            File.WriteAllText(outPath, $"# {gmd}\r\n# {mat.Name}\r\n" + File.ReadAllText(outPath));

                            foreach (var line in File.ReadAllLines(outPath).Where(x => x.StartsWith("#") || x.StartsWith("Flags:") || x.StartsWith("  GeometryFlags:") || x.StartsWith("  VertexAttributeFlags:")))
                                lines.Add(line);
                            lines.Add("\r\n");
                        }
                    }
                }
                catch { Console.WriteLine($"Error opening {gmd}"); }
            }

            File.WriteAllLines("output.txt", lines);
        }
    }
}
