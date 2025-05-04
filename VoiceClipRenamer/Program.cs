using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceClipRenamer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RenameFilesFromTxt(args[0], args[1], args[2]);
        }

        private static void RenameFilesFromTxt(string directory, string txtFile, string outDir)
        {
            var txtLines = File.ReadAllLines(txtFile).ToList();

            Directory.CreateDirectory(outDir);
            var files = Directory.GetFiles(directory, "*.wav", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i], Path.Combine(outDir, txtLines[i]));
            }
        }

        private static void SanitizeNameListFromSpreadsheetColumn(string txtFile)
        {
            List<string> newLines = new List<string>();
            var lines = File.ReadAllLines(txtFile).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                string fileName = i + "_" + lines[i].Replace(" ", "").Replace("?", "").Replace(".", "").Replace(",", "").Replace("!", "").Replace("'", "").ToLower() + ".wav";
                newLines.Add(fileName);
            }
            File.WriteAllLines("newlines.txt", newLines);
        }
    }
}
