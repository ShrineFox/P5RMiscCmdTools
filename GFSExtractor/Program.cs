using ShrineFox.IO;
using System.Text;

namespace GFSExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DumpGMDs(args[0]);

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static void DumpGMDs(string dirPath)
        {
            foreach (var file in Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories).Where(x =>
                x.ToLower().EndsWith(".epl") || x.ToLower().EndsWith(".gap")))
            {

                List<long> positions = StringSearch.Program.FindStringInBinaryFile(file, "GFS0", Encoding.UTF8);

                foreach (long position in positions)
                {
                    if (position > -1)
                    {
                        if (position - 16 > 0)
                        {
                            using (FileStream fs = new FileStream(file, FileMode.Open))
                            using (EndianBinaryReader br = new EndianBinaryReader(fs, Endianness.BigEndian))
                            {
                                var result = ReadStringBackwards(br, position - 16);

                                if (!string.IsNullOrEmpty(result.Item1))
                                {
                                    long extPos = result.Item2 + 1;
                                    string extFileName = result.Item1.Substring(1, result.Item1.Length - 2);

                                    if ((extFileName.ToLower().EndsWith(".gmd") || extFileName.ToLower().EndsWith(".gfs"))
                                        && extPos > 0)
                                    {
                                        br.BaseStream.Position = position - 4;
                                        uint size = br.ReadUInt32();

                                        byte[] extractedFile = br.ReadBytes(Convert.ToInt32(size));
                                        string outPath = Path.Combine($".//ExtractedGMDs//{Path.GetFileName(file)}", extFileName);
                                        Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                                        File.WriteAllBytes(outPath, extractedFile);
                                        Console.WriteLine(outPath);
                                    }

                                }

                            }
                        }
                    }
                }
            }
        }

        static (string, long) ReadStringBackwards(EndianBinaryReader reader, long position)
        {
            if (position >= reader.BaseStream.Length || position < 0)
                return ("", -1);

            StringBuilder sb = new StringBuilder();
            long startPosition = position;
            reader.BaseStream.Seek(position, SeekOrigin.Begin);

            while (startPosition >= 0)
            {
                reader.BaseStream.Seek(startPosition, SeekOrigin.Begin);
                byte b = reader.ReadByte();

                if (b == 0) // Stop at the first non-ASCII character
                    break;

                sb.Insert(0, (char)b); // Prepend character
                startPosition--;
            }

            return (sb.ToString(), startPosition + 1);
        }
    }
}
