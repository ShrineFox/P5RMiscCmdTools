using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDummyFiles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string scriptDir = currentDir + "\\BF";
            string dummyDir = currentDir + "\\CPK\\DUMMYFILES.CPK";

            foreach (var file in Directory.GetFiles(scriptDir, "*", SearchOption.AllDirectories)
                .Where(x => !x.Contains("_CustomScripts") && !x.Contains(".CPK") && 
                    (x.ToLower().EndsWith(".msg") || x.ToLower().EndsWith(".flow")) ))
            {
                string relativePath = file.Remove(0, scriptDir.Length);
                string dummyPath = dummyDir + "\\" + relativePath.Replace(".flow",".BF").Replace(".msg",".BF");

                if (!Directory.Exists(Path.GetDirectoryName(dummyPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(dummyPath));

                if (!File.Exists(dummyPath))
                {
                    File.CreateText(dummyPath);
                    Console.WriteLine($"Created: {dummyPath}");
                }
            }
            Console.WriteLine($"Finished creating dummy .BF files in DUMMYFILES.CPK folder.");

            foreach (var dummyFile in Directory.GetFiles(dummyDir, "*", SearchOption.AllDirectories)
                .Where(x => x.ToLower().EndsWith(".bf")))
            {
                string flowPath = dummyFile.Replace($"CPK\\DUMMYFILES.CPK\\","BF\\").Replace(".bf", ".flow").Replace(".BF", ".flow");

                if (!File.Exists(flowPath))
                {
                    try
                    {
                        File.Delete(dummyFile);
                        Console.WriteLine($"Deleted: {dummyFile}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{e.Message}\n\tFailed to delete file: {dummyFile}");
                    }
                }
            }
            Console.WriteLine($"Finished deleting dummy .BF files in DUMMYFILES.CPK folder.");

            string modFolderRoot = Path.GetDirectoryName(Path.GetDirectoryName(currentDir));
            string bindFolder = Path.Combine(modFolderRoot, "crifs.v2.hook//Bind");
            string cacheFolder = Path.Combine(modFolderRoot, "p5rpc.modloader//Cache");

            /*
            if (Directory.Exists(bindFolder))
            {
                Directory.Delete(bindFolder, true);
                Console.WriteLine($"Deleted crifs.v2.hook\\Bind folder.");
            }
            if (Directory.Exists(cacheFolder))
            {
                Directory.Delete(cacheFolder, true);
                Console.WriteLine($"Deleted p5rpc.modloader\\Cache folder.");
            }
            */
        }
    }
}
