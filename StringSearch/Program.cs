using System;
using System.IO;
using System.Text;

namespace StringSearch
{
    public class Program
    {
        static void Main(string[] args)
        {
            string searchPattern = args[2];
            foreach (var file in Directory.GetFiles(args[0], args[1], SearchOption.AllDirectories))
            {
                long position = ContainsText(file, searchPattern);

                if (position > -1)
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

        public static long ContainsText(string filePath, string searchText)
        {
            byte[] searchBytes = Encoding.UTF8.GetBytes(searchText);
            int bufferSize = 4096; // Adjust as needed
            byte[] buffer = new byte[bufferSize];
            byte[] overlap = new byte[searchBytes.Length - 1];

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead;
                long currentFilePosition = 0; // Track file position manually

                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Combine overlap with the new buffer
                    byte[] combinedBuffer = CombineBuffers(overlap, buffer, bytesRead);

                    // Search for the pattern in the combined buffer
                    int matchIndex = SearchInBuffer(combinedBuffer, searchBytes);
                    if (matchIndex != -1)
                    {
                        return currentFilePosition + matchIndex; // Return the correct start position
                    }

                    // Update file position tracking
                    currentFilePosition += bytesRead - overlap.Length;

                    // Save the last part of the buffer for overlap
                    Array.Copy(buffer, bytesRead - overlap.Length, overlap, 0, overlap.Length);
                }
            }

            return -1; // No match found
        }

        public static int SearchInBuffer(byte[] buffer, byte[] pattern)
        {
            for (int i = 0; i <= buffer.Length - pattern.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (buffer[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return i;
            }
            return -1;
        }

        private static byte[] CombineBuffers(byte[] overlap, byte[] buffer, int bytesRead)
        {
            byte[] combined = new byte[overlap.Length + bytesRead];
            Buffer.BlockCopy(overlap, 0, combined, 0, overlap.Length);
            Buffer.BlockCopy(buffer, 0, combined, overlap.Length, bytesRead);
            return combined;
        }

        public static List<long> FindAllOccurrences(string filePath, string searchText)
        {
            byte[] searchBytes = Encoding.UTF8.GetBytes(searchText);
            int bufferSize = 4096; // Adjust as needed
            byte[] buffer = new byte[bufferSize];
            byte[] overlap = new byte[searchBytes.Length - 1];
            List<long> positions = new List<long>();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead;
                long currentFilePosition = 0; // Track file position manually

                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Combine overlap with the new buffer
                    byte[] combinedBuffer = CombineBuffers(overlap, buffer, bytesRead);

                    // Search for all occurrences in the buffer
                    List<int> matchIndexes = SearchAllInBuffer(combinedBuffer, searchBytes);
                    foreach (int matchIndex in matchIndexes)
                    {
                        positions.Add(currentFilePosition + matchIndex);
                    }

                    // Update file position tracking
                    currentFilePosition += bytesRead - overlap.Length;

                    // Save the last part of the buffer for overlap
                    Array.Copy(buffer, bytesRead - overlap.Length, overlap, 0, overlap.Length);
                }
            }

            return positions; // Return all match positions
        }

        public static List<long> FindStringInBinaryFile(string filePath, string searchString, Encoding encoding)
        {
            List<long> positions = new List<long>();
            byte[] searchBytes = encoding.GetBytes(searchString);
            byte firstByte = searchBytes[0];

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);

                for (long i = 0; i <= buffer.Length - searchBytes.Length; i++)
                {
                    if (buffer[i] == firstByte)
                    {
                        bool match = true;
                        for (int j = 1; j < searchBytes.Length; j++)
                        {
                            if (buffer[i + j] != searchBytes[j])
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                        {
                            positions.Add(i);
                        }
                    }
                }
            }
            return positions;
        }

        public static List<int> SearchAllInBuffer(byte[] buffer, byte[] pattern)
        {
            List<int> positions = new List<int>();
            for (int i = 0; i <= buffer.Length - pattern.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (buffer[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) positions.Add(i); // Store match index
            }
            return positions;
        }
    }

}
