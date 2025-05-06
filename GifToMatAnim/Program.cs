using GFDLibrary;
using GFDLibrary.Models;
using GFDLibrary.Textures;
using GFDStudio.FormatModules;
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
                var texture = TextureEncoder.Encode(Path.GetFileNameWithoutExtension(args[0]) + ".dds", TextureFormat.DDS, composite);

                // Inject into GMD
                var gmd = ModuleImportUtilities.ImportFile<ModelPack>("./GMD/IT0420_072.GMD");
                gmd.Textures.Textures.First().Data = texture.Data;
                gmd.Textures.Textures.First().Name = texture.Name;
                gmd.Materials.Materials[0].DiffuseMap.Name = texture.Name;
                gmd.Materials.Materials[0].DiffuseMap.Flags = 1; // Field44, makes texture animate
                gmd.Materials.Materials[0].Name = Path.GetFileNameWithoutExtension(texture.Name);
                foreach (var node in gmd.Model.Nodes.Where(x => x.HasAttachments 
                && x.Attachments.Any(y => y.Type == GFDLibrary.Models.NodeAttachmentType.Mesh)))
                {
                    foreach(NodeMeshAttachment mesh in node.Attachments.Where(y => y.Type == GFDLibrary.Models.NodeAttachmentType.Mesh))
                    {
                        mesh.Mesh.MaterialName = gmd.Materials.Materials[0].Name;
                    }
                }
                gmd.AnimationPack.Animations[0].Controllers[0].TargetName = gmd.Materials.Materials[0].Name;
                Directory.CreateDirectory("./Output/");
                gmd.Save("./Output/IT0420_072.GMD");
                Console.WriteLine($"Saved GMD file: ./Output/IT0420_072.GMD");
                Console.ReadKey();
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