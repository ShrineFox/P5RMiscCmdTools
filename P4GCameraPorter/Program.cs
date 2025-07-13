using AtlusFileSystemLibrary.FileSystems.PAK;
using System.IO.Compression;

namespace P4GCameraPorter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // args: p4g_pc paks dir, replacement cmr file, output folder

            foreach(var pakFile in Directory.GetFiles(args[0]))
            {
                PAKFileSystem pak = new PAKFileSystem();
                if (PAKFileSystem.TryOpen(pakFile, out pak))
                {
                    List<string> cmrFiles = new List<string>();
                    foreach (var cmrFile in pak.EnumerateFiles().Where(x => x.ToLower().EndsWith(".cmr")))
                    {
                        string outPath = Path.Combine(args[2], Path.GetFileName(pakFile), cmrFile);
                        Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                        File.Copy(args[1], outPath, true);
                    }
                }
            }
        }
    }
}
