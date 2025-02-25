using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearReloadedCache
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string modFolderRoot = Path.GetDirectoryName(currentDir);
            string bindFolder = Path.Combine(modFolderRoot, "crifs.v2.hook//Bind");
            string cacheFolder = Path.Combine(modFolderRoot, "p5rpc.modloader//Cache");

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
        }
    }
}
