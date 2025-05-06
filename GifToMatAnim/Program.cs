using GFDLibrary;
using GFDLibrary.Textures;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace GifToMatAnim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputGif = args[0];
            string outputDDS = "./output.dds";

            using (Image gif = Image.FromFile(inputGif))
            {
                FrameDimension dimension = new FrameDimension(gif.FrameDimensionsList[0]);
                int totalFrames = gif.GetFrameCount(dimension);

                int targetFrameCount = 21;
                int[] frameIndices = Enumerable.Range(0, targetFrameCount)
                                               .Select(i => (int)Math.Round(i * (totalFrames - 1) / (double)(targetFrameCount - 1)))
                                               .Distinct()
                                               .ToArray();

                // Step 1: Determine ideal 16:9 resolution closest to original
                gif.SelectActiveFrame(dimension, 0);
                Size targetSize = GetNearest16by9Resolution(gif.Width, gif.Height);

                // Step 2: Resize and collect frames
                Bitmap[] frames = new Bitmap[frameIndices.Length];
                for (int i = 0; i < frameIndices.Length; i++)
                {
                    gif.SelectActiveFrame(dimension, frameIndices[i]);
                    using (var original = new Bitmap(gif))
                    {
                        Bitmap resized = new Bitmap(targetSize.Width, targetSize.Height);
                        using (Graphics g = Graphics.FromImage(resized))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(original, 0, 0, targetSize.Width, targetSize.Height);
                        }
                        frames[i] = resized;
                    }
                }

                // Step 3: Create final composite image (horizontal strip)
                int finalWidth = targetSize.Width * frames.Length;
                int finalHeight = targetSize.Height;
                Bitmap composite = new Bitmap(finalWidth, finalHeight);

                using (Graphics g = Graphics.FromImage(composite))
                {
                    g.Clear(Color.Transparent);
                    for (int i = 0; i < frames.Length; i++)
                    {
                        g.DrawImage(frames[i], i * targetSize.Width, 0);
                        frames[i].Dispose();
                    }
                }

                // Convert to DDS
                var texture = TextureEncoder.Encode("temp.dds", TextureFormat.DDS, composite);
                File.WriteAllBytes(outputDDS, texture.Data);
                Console.WriteLine($"Saved {frameIndices.Length} frames into DDS file: {outputDDS}");
            }
        }

        // Ensures 16:9 and both width/height are multiples of 4
        static Size GetNearest16by9Resolution(int originalWidth, int originalHeight)
        {
            // Start with the smaller dimension to avoid enlarging too much
            int height = (int)Math.Round(originalHeight / 4.0) * 4;
            int width = (int)Math.Round((height * 16.0 / 9.0) / 4.0) * 4;

            return new Size(width, height);
        }
    }
}