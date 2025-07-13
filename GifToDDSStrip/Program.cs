using GFDLibrary;
using GFDLibrary.Models;
using GFDLibrary.Textures;
using GFDStudio.FormatModules;
using Scarlet.IO.ImageFormats;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GifToDDSStrip
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string inputDir = args[0];
            string outDir = args[1];

            foreach (var img in Directory.GetFiles(args[0]).Where(x => Path.GetExtension(x).ToLower() != ".dds"))
            {
                string outImgPath = Path.Combine(outDir, Path.GetFileNameWithoutExtension(img)) + ".dds";

                if (Path.GetExtension(img).ToLower() == ".gif")
                {
                    using (Image gif = Image.FromFile(img))
                    {
                        FrameDimension dimension = new FrameDimension(gif.FrameDimensionsList[0]);
                        int totalFrames = gif.GetFrameCount(dimension);

                        Bitmap[] frames = new Bitmap[totalFrames];
                        for (int i = 0; i < totalFrames; i++)
                        {
                            gif.SelectActiveFrame(dimension, i);
                            using (var original = new Bitmap(gif))
                            {
                                Bitmap resized = new Bitmap(gif.Width, gif.Height);
                                using (Graphics g = Graphics.FromImage(resized))
                                {
                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    g.DrawImage(original, 0, 0, gif.Width, gif.Height);
                                }
                                frames[i] = resized;
                            }
                        }

                        // Step 3: Create final composite image (horizontal strip)
                        int finalWidth = gif.Width * frames.Length;
                        int finalHeight = gif.Height;
                        Bitmap composite = new Bitmap(finalWidth, finalHeight);

                        using (Graphics g = Graphics.FromImage(composite))
                        {
                            g.Clear(Color.Transparent);
                            for (int i = 0; i < frames.Length; i++)
                            {
                                g.DrawImage(frames[i], i * gif.Width, 0);
                                frames[i].Dispose();
                            }
                        }

                        // Convert to DDS and save as new file
                        var texture = TextureEncoder.Encode(Path.GetFileNameWithoutExtension(args[0]) + ".dds", TextureFormat.DDS, composite);

                        // Write DDS data, skipping the first 32 bytes in the output file
                        using (MemoryStream ms = new MemoryStream())
                        {
                            texture.Save(ms, true);

                            ms.Seek(Search(ms, new byte[] {0x44, 0x44, 0x53, 0x20 }), SeekOrigin.Begin);

                            // Create a FileStream to write the data
                            using (var fileStream = new FileStream(outImgPath, FileMode.Create, FileAccess.Write))
                            {
                                // Copy the remaining data from the MemoryStream to the FileStream
                                ms.CopyTo(fileStream);
                            }
                        }
                    }
                }
                else
                {
                    using (Bitmap bmp = new Bitmap(img))
                    {
                        // Convert to DDS and save as new file
                        var texture = TextureEncoder.Encode(Path.GetFileNameWithoutExtension(args[0]) + ".dds", TextureFormat.DDS, bmp);

                        // Write DDS data, skipping the first 32 bytes in the output file
                        using (MemoryStream ms = new MemoryStream())
                        {
                            texture.Save(ms, true);

                            ms.Seek(Search(ms, new byte[] { 0x44, 0x44, 0x53, 0x20 }), SeekOrigin.Begin);

                            // Create a FileStream to write the data
                            using (var fileStream = new FileStream(outImgPath, FileMode.Create, FileAccess.Write))
                            {
                                // Copy the remaining data from the MemoryStream to the FileStream
                                ms.CopyTo(fileStream);
                            }
                        }
                    }

                }
            }
        }

        public static long Search(Stream stream, byte[] pattern)
        {
            if (pattern == null || pattern.Length == 0)
                throw new ArgumentException("Pattern must be non-empty", nameof(pattern));

            stream.Seek(0, SeekOrigin.Begin);

            int b;
            long position = 0;
            int matched = 0;

            while ((b = stream.ReadByte()) != -1)
            {
                if (b == pattern[matched])
                {
                    matched++;
                    if (matched == pattern.Length)
                        return position - pattern.Length + 1;
                }
                else
                {
                    // If partial match, need to re-check for overlapping patterns
                    if (matched > 0)
                    {
                        // Rewind stream to re-check for overlapping pattern
                        stream.Position = position - matched + 1;
                        position = stream.Position;
                        matched = 0;
                        continue;
                    }
                }
                position++;
            }
            return -1; // Not found
        }
    }
}