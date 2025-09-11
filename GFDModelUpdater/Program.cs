using GFDLibrary;
using GFDLibrary.Models;

namespace GFDModelUpdater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string ogGMDPath = args[0];
            string editedGMDPath = args[1];
            string outGMDPath = args[2];

            ModelPack ogGMD = Resource.Load<ModelPack>(ogGMDPath);
            ModelPack editedGMD = Resource.Load<ModelPack>(editedGMDPath);
            ModelPack outGMD = Resource.Load<ModelPack>(editedGMDPath);

            // Add textures from og GMD that aren't in edited GMD
            foreach (var texture in ogGMD.Textures)
            {
                if (!editedGMD.Textures.Values.Any(x => x.Name == texture.Value.Name))
                {
                    try
                    {
                        outGMD.Textures.Add(texture.Value.Name, texture.Value);
                        Console.WriteLine($"Added Texture \"{texture.Value.Name}\" to GMD", ConsoleColor.Yellow);
                    }
                    catch { }
                }
            }

            // Add materials from og GMD that aren't in edited GMD (only if a mesh uses it)
            foreach (var mat in ogGMD.Materials)
            {
                if (!editedGMD.Materials.Values.Any(x => x.Name == mat.Value.Name))
                {
//                    if (editedGMD.Model.Meshes.Any(x => x.MaterialName == mat.Value.Name))
                    {
                        outGMD.Materials.Add(mat.Value.Name, mat.Value);
                        Console.WriteLine($"Added Material \"{mat.Value.Name}\"", ConsoleColor.Blue);
                    }
                }
            }

            // Update materials that are in both GMDs using old properties
            foreach (var mat in editedGMD.Materials)
            {
                if (ogGMD.Materials.Values.Any(x => x.Name == mat.Value.Name))
                {
                    try
                    {
                        var matchingMat = ogGMD.Materials.Values.First(x => x.Name == mat.Value.Name);
                        outGMD.Materials.Remove(mat.Value);
                        outGMD.Materials.Add(matchingMat.Name, matchingMat);
                        Console.WriteLine($"Updated Material \"{mat.Value.Name}\"", ConsoleColor.Blue);
                    }
                    catch { }
                }
            }



            // Save the updated GMD
            outGMD.Save(outGMDPath);
            Console.WriteLine($"\n\nSaved new GMD to: \"{outGMDPath}\"");
        }
    }
}
