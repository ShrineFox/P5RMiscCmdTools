
using System.Diagnostics;
using AtlusFileSystemLibrary.FileSystems.PAK;
using Newtonsoft.Json;

namespace CreateLooseBINs
{
    internal class Program
    {
        public static List<BINFile> bins = new List<BINFile>();

        public class BINFile 
        {
            public string Name { get; set; } = "";
            public List<string> DDSFiles { get; set; } = new List<string>();
        }

        static void Main(string[] args)
        {
            //bins = GetBINFileList(args[0]);
            //File.WriteAllText("CreateLooseBINs.json", JsonConvert.SerializeObject(bins, Newtonsoft.Json.Formatting.Indented));

            string currentFolder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string binJsonPath = Path.Combine(currentFolder, "CreateLooseBINs.json");
            if (File.Exists(binJsonPath))
                bins = LoadBINJson(binJsonPath);
            else
                Console.WriteLine($"Failed to locate file: {binJsonPath}");
            string devTexturesFolder = Path.Combine(currentFolder, @"_DevStuff");
            string looseBINsFolder = Path.Combine(currentFolder, @"LooseBINs/MODEL/FIELD_TEX/TEXTURES");

            try
            {
                foreach (var devTex in Directory.GetFiles(devTexturesFolder, "*.dds", SearchOption.AllDirectories))
                {
                    foreach (var matchingTexBin in bins.Where(x => x.DDSFiles.Any(y => Path.GetFileNameWithoutExtension(devTex).Equals(Path.GetFileNameWithoutExtension(y)))))
                    {
                        string outputPath = Path.Combine(looseBINsFolder, matchingTexBin.Name + ".BIN", Path.GetFileName(devTex));
                        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                        Console.WriteLine($"Copying {Path.GetFileName(devTex)} to {matchingTexBin.Name}");
                        File.Copy(devTex, outputPath, true);
                    }
                }
                Console.WriteLine($"\nDone copying files! Press any key to exit.");
            }
            catch(Exception ex) { Console.WriteLine(ex.ToString()); }
            
            Console.ReadKey();
        }

        private static List<BINFile> LoadBINJson(string binJsonPath)
        {
            return JsonConvert.DeserializeObject<List<BINFile>>(File.ReadAllText(Path.GetFullPath(binJsonPath)));
        }

        private static List<BINFile> GetBINFileList(string binFolder)
        {
            List<BINFile> binFileList = new List<BINFile>();

            foreach (var file in Directory.GetFiles(binFolder, "*.BIN", SearchOption.TopDirectoryOnly))
            {
                BINFile binFile = new BINFile() { Name = Path.GetFileNameWithoutExtension(file) };

                PAKFileSystem pak = new PAKFileSystem();
                if (PAKFileSystem.TryOpen(file, out pak))
                {
                    List<string> pakFiles = new List<string>();
                    foreach (var pakFile in pak.EnumerateFiles().Where(x => x.ToLower().EndsWith(".dds")))
                    {
                        binFile.DDSFiles.Add(pakFile.Replace("../", "")); //Remove backwards relative path
                    }
                }
                binFileList.Add(binFile);
            }

            return binFileList;
        }
    }
}
