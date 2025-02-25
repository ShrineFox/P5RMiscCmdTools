using System;
using System.IO;
using System.Text;

namespace StringSearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string searchPattern = args[2];
            foreach (var file in Directory.GetFiles(args[0], args[1], SearchOption.AllDirectories))
            {
                if (ContainsText(file, searchPattern))
                {
                    Console.WriteLine(file);
                    CopyToOutDir(file);
                }
            }
        }

        private static void CopyToOutDir(string file)
        {
            string outDir = "./Matches/";
            Directory.CreateDirectory(outDir);
            File.Copy(file, Path.Combine(outDir, Path.GetFileName(file)));
        }

        public static bool ContainsText(string filePath, string searchText)
        {
            // Convert the search text to a byte array using the desired encoding
            byte[] searchBytes = Encoding.UTF8.GetBytes(searchText);
            int bufferSize = 4096; // Adjust as needed
            byte[] buffer = new byte[bufferSize];
            byte[] overlap = new byte[searchBytes.Length - 1];

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Combine overlap with the new buffer
                    byte[] combinedBuffer = CombineBuffers(overlap, buffer, bytesRead);

                    // Check for the presence of the search pattern
                    if (SearchInBuffer(combinedBuffer, searchBytes))
                    {
                        return true; // Match found
                    }

                    // Save the last part of the buffer for overlap
                    Array.Copy(buffer, bytesRead - overlap.Length, overlap, 0, overlap.Length);
                }
            }
            return false; // No match found
        }

        private static bool SearchInBuffer(byte[] buffer, byte[] searchBytes)
        {
            for (int i = 0; i <= buffer.Length - searchBytes.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < searchBytes.Length; j++)
                {
                    if (buffer[i + j] != searchBytes[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return true;
                }
            }
            return false;
        }

        private static byte[] CombineBuffers(byte[] overlap, byte[] buffer, int bytesRead)
        {
            byte[] combined = new byte[overlap.Length + bytesRead];
            Buffer.BlockCopy(overlap, 0, combined, 0, overlap.Length);
            Buffer.BlockCopy(buffer, 0, combined, overlap.Length, bytesRead);
            return combined;
        }
    }

}
