using System.Diagnostics;
using System.IO.Compression;
using System.Windows.Forms;
using AtlusFileSystemLibrary;
using AtlusFileSystemLibrary.FileSystems.PAK;

namespace RepackBINs
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            try
            {
                string currentFolder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                string looseBINsFolder = Path.Combine(currentFolder, @"LooseBINs/MODEL/FIELD_TEX/TEXTURES");
                string repackedBINsFolder = Path.Combine(currentFolder, @"RepackedBINs\TEX_WIP.CPK\MODEL\FIELD_TEX\TEXTURES");
                string originalBINsFolder = "";
                FolderBrowserDialog folderDialog = new FolderBrowserDialog() { UseDescriptionForTitle = true, Description = "Choose Unedited, Original .BINs Folder" };
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    originalBINsFolder = folderDialog.SelectedPath;
                }
                else
                    return;

                foreach (var looseBIN in Directory.GetDirectories(looseBINsFolder))
                {
                    PAKFileSystem pak = new PAKFileSystem();

                    string matchingOriginalBINFile = Path.Combine(originalBINsFolder, Path.GetFileName(looseBIN.Replace(".bin","").Replace(".BIN",""))) + ".BIN";
                    if (PAKFileSystem.TryOpen(matchingOriginalBINFile, out pak))
                    {
                        foreach (var looseBinDds in Directory.GetFiles(looseBIN))
                        {
                            pak.AddFile(Path.GetFileName(looseBinDds), looseBinDds, ConflictPolicy.Replace);
                        }
                        string outputPath = Path.Combine(repackedBINsFolder, Path.GetFileName(matchingOriginalBINFile));
                        Console.WriteLine($"Repacking {outputPath}");
                        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                        pak.Save(outputPath);
                    }
                    else
                        Console.WriteLine($"Failed to open original BIN: {matchingOriginalBINFile}");
                }
                Console.WriteLine($"\nDone repacking BINs! Press any key to exit.");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            
            Console.ReadKey();
        }
    }
}