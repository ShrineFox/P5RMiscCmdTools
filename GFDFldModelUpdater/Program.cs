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

            // Remove models from og GFS that are not in editedGFS (or have all their attachments removed)
            foreach (var node in ogGFS.Model.Nodes)
            {
                if (!editedGFS.Model.Nodes.Any(x => x.Name == node.Name) || 
                    ( editedGFS.Model.Nodes.First(x => x.Name == node.Name).AttachmentCount == 0 
                        && node.AttachmentCount > 0))
                {
                    outGFS.Model.Nodes.First(x => x.Name == node.Name).Parent.RemoveChildNode(node);
                    Console.WriteLine($"Removed Node \"{node.Name}\" from GFS", ConsoleColor.Red);
                }
            }

            // Add models that aren't in og GFS
            foreach (var node in editedGFS.Model.Nodes)
            {
                // Skip nodes with Camera attachments, properties containing "Cmr", or names in the exclusion list
                if (node.Attachments.Any(x => x.Type == NodeAttachmentType.Camera) ||
                    node.Properties.Any(x => x.Key.Contains("Cmr")) ||
                    (!string.IsNullOrEmpty(exclusions) && exclusions.Split(',').Any(y => node.Name.Contains(y))))
                {
                    Console.WriteLine($"Skipping Node \"{node.Name}\"...", ConsoleColor.DarkGray);
                    continue;
                }

                var outNode = outGFS.Model.Nodes.FirstOrDefault(x => x.Name == node.Name);
                var ogNode = ogGFS.Model.Nodes.FirstOrDefault(x => x.Name == node.Name);

                if (ogNode == null)
                {
                    // If the node doesn't exist in the original GFS, add it
                    outGFS.Model.RootNode.AddChildNode(node);
                    Console.WriteLine($"Added Node \"{node.Name}\" to GFS", ConsoleColor.Green);
                }
                else if (outNode.HasAttachments
                    && outNode.Attachments != null
                    && outNode.Attachments.Where(x => x.Type == NodeAttachmentType.Mesh).Count() != 0)
                {
                    // If node has any mesh attachments...
                    
                    var editedMeshAttachments = node.Attachments
                        .Where(x => x.Type == NodeAttachmentType.Mesh)
                        .ToList();
                    List<NodeMeshAttachment> editedMeshes = new();
                    foreach (var attachment in editedMeshAttachments)
                        editedMeshes.Add((NodeMeshAttachment)attachment);

                    var ogMeshAttachments = ogNode.Attachments
                        .Where(x => x.Type == NodeAttachmentType.Mesh)
                        .ToList();
                    List<NodeMeshAttachment> ogMeshes = new();
                    foreach (var attachment in ogMeshAttachments)
                        ogMeshes.Add((NodeMeshAttachment)attachment);

                    var outMeshAttachments = outNode.Attachments
                        .Where(x => x.Type == NodeAttachmentType.Mesh)
                        .ToList();
                    List<NodeMeshAttachment> outMeshes = new();
                    foreach (var attachment in outMeshAttachments)
                        outMeshes.Add((NodeMeshAttachment)attachment);

                    // Add new mesh attachments with unique material names
                    foreach (var attachment in editedMeshes)
                    {
                        var materialName = attachment.Mesh.MaterialName;
                        if (!ogMeshes.Any(x => x.Mesh.MaterialName == materialName))
                        {
                            outNode.Attachments.Add(attachment);
                            Console.WriteLine($"Added Mesh Attachment with Material \"{materialName}\" to Node \"{node.Name}\"", ConsoleColor.Green);
                        }
                    }

                    // Remove mesh attachments with material names that have fewer occurrences in editedGFS
                    var materialGroups = ogMeshes.GroupBy(x => x.Mesh.MaterialName);
                    foreach (var group in materialGroups)
                    {
                        var materialName = group.Key;
                        var editedCount = editedMeshes.Count(x => x.Mesh.MaterialName == materialName);
                        var ogCount = group.Count();

                        if (editedCount < ogCount)
                        {
                            foreach (var attachment in ogNode.Attachments)
                                if (attachment.Type == NodeAttachmentType.Mesh)
                                {
                                    var temp = attachment as NodeMeshAttachment;
                                    if (temp.Mesh.MaterialName == materialName)
                                        outNode.Attachments.Remove(attachment);
                                }
                            Console.WriteLine($"Removed Mesh Attachments with Material \"{materialName}\" from Node \"{node.Name}\"", ConsoleColor.Red);                        }
                    }
                }
            }

            // Remove textures that aren't in edited GFS
            /*
            foreach (var texture in ogGFS.Textures)
            {
                if (!editedGFS.Textures.Values.Any(x => x.Name == texture.Value.Name))
                {
                    outGFS.Textures.Remove(texture);
                    Console.WriteLine($"Removed Texture \"{texture.Value.Name}\" from GFS", ConsoleColor.DarkYellow);
                }
            }
            */

            // Add textures that aren't in og GFS
            foreach (var texture in editedGFS.Textures)
            {
                if (!ogGFS.Textures.Values.Any(x => x.Name == texture.Value.Name))
                {
                    try
                    {
                        outGFS.Textures.Add(texture.Value.Name, texture.Value);
                        Console.WriteLine($"Added Texture \"{texture.Value.Name}\" to GFS", ConsoleColor.Yellow);
                    }
                    catch { }
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
                    try
                    {
                        outGFS.Materials.Add(mat.Value.Name, mat.Value);
                        Console.WriteLine($"Added Material \"{mat.Value.Name}\" to GFS", ConsoleColor.Blue);
                    }
                    catch { }
                }
            }

            // Save the updated GFS
            outGFS.Save(outGFSPath);
            Console.WriteLine($"\n\nSaved new GFS to: \"{outGFSPath}\"");
            Console.ReadKey();
        }
    }
}
