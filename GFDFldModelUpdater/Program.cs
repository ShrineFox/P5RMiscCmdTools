using GFDLibrary;
using GFDLibrary.Models;

namespace GFDFldModelUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ogGFSPath = args[0];
            string editedGFSPath = args[1];
            string outGFSPath = args[2];
            string exclusions = "";

            if (args.Length == 4)
                exclusions = args[3];

            ModelPack ogGFS = Resource.Load<ModelPack>(ogGFSPath);
            ModelPack editedGFS = Resource.Load<ModelPack>(editedGFSPath);
            ModelPack outGFS = Resource.Load<ModelPack>(editedGFSPath);

            // Remove models from og GFS that are not in editedGFS
            foreach (var node in ogGFS.Model.Nodes)
            {
                if (!editedGFS.Model.Nodes.Any(x => x.Name == node.Name))
                {
                    outGFS.Model.Nodes.First(x => x.Name == node.Name).Parent.RemoveChildNode(node);
                    Console.WriteLine($"Removed Node \"{node.Name}\" from GFS", ConsoleColor.Red);
                }
            }

            // Add models that aren't in og GFS
            foreach (var node in editedGFS.Model.Nodes)
            {
                if (!ogGFS.Model.Nodes.Any(x => x.Name == node.Name))
                {
                    outGFS.Model.RootNode.AddChildNode(node);
                    Console.WriteLine($"Added Node \"{node.Name}\" to GFS", ConsoleColor.Green);
                }
                else if (!node.Properties.Any(x => x.Key.Contains("Cmr")) 
                    && (string.IsNullOrEmpty(exclusions) || !exclusions.Split(',').Any(y => node.Name.Contains(y)))
                    && !node.Attachments.Any(x => x.Type == NodeAttachmentType.Camera))
                {
                    // If node isn't camera-related or part of exclusions list, replace attachments
                    outGFS.Model.Nodes.First(x => x.Name == node.Name).Attachments = node.Attachments;
                    Console.WriteLine($"Updated Node Attachments for \"{node.Name}\" in GFS", ConsoleColor.DarkGreen);
                }
            }

            // Remove textures that aren't in edited GFS
            foreach (var texture in ogGFS.Textures)
            {
                if (!editedGFS.Textures.Values.Any(x => x.Name == texture.Value.Name))
                {
                    outGFS.Textures.Remove(texture);
                    Console.WriteLine($"Removed Texture \"{texture.Value.Name}\" from GFS", ConsoleColor.DarkYellow);
                }
            }

            // Add textures that aren't in og GFS
            foreach (var texture in editedGFS.Textures)
            {
                if (!ogGFS.Textures.Values.Any(x => x.Name == texture.Value.Name))
                {
                    outGFS.Textures.Add(texture.Value.Name, texture.Value);
                    Console.WriteLine($"Added Texture \"{texture.Value.Name}\" to GFS", ConsoleColor.Yellow);
                }
            }

            // Remove materials that aren't in edited GFS
            foreach (var mat in ogGFS.Materials)
            {
                if (!editedGFS.Materials.Values.Any(x => x.Name == mat.Value.Name))
                {
                    outGFS.Materials.Remove(mat.Value.Name);
                    Console.WriteLine($"Removed Material \"{mat.Value.Name}\" from GFS", ConsoleColor.DarkBlue);
                }
            }

            // Add materials that aren't in og GFS
            foreach (var mat in editedGFS.Materials)
            {
                if (!ogGFS.Materials.Values.Any(x => x.Name == mat.Value.Name))
                {
                    outGFS.Materials.Add(mat.Value.Name, mat.Value);
                    Console.WriteLine($"Added Material \"{mat.Value.Name}\" to GFS", ConsoleColor.Blue);
                }
            }

            // Save the updated GFS
            outGFS.Save(outGFSPath);
            Console.WriteLine($"\n\nSaved new GFS to: \"{outGFSPath}\"");
            Console.ReadKey();
        }
    }
}
