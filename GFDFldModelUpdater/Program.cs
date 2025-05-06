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

            ModelPack ogGFS = Resource.Load<ModelPack>(ogGFSPath);
            ModelPack editedGFS = Resource.Load<ModelPack>(editedGFSPath);
            ModelPack outGFS = Resource.Load<ModelPack>(editedGFSPath);

            // Remove nodes from og GFS that are not in editedGFS (or have all their attachments removed)
            foreach (var node in ogGFS.Model.Nodes)
            {
                if (!editedGFS.Model.Nodes.Any(x => x.Name == node.Name))
                {
                    outGFS.Model.Nodes.First(x => x.Name == node.Name).Parent.RemoveChildNode(node);
                    Console.WriteLine($"Removed Node \"{node.Name}\" from GFS (not present in edited GMD)", ConsoleColor.Red);
                }
            }

            // Add models that aren't in og GFS
            foreach (var node in editedGFS.Model.Nodes)
            {
                var outNode = outGFS.Model.Nodes.FirstOrDefault(x => x.Name == node.Name);
                var ogNode = ogGFS.Model.Nodes.FirstOrDefault(x => x.Name == node.Name);

                if (ogNode == null)
                {
                    // If the node doesn't exist in the original GFS, add it
                    // (but first set the mesh field14 to 0 if it is 69)
                    List<NodeAttachment> newMeshNodeAttachments = new();
                    foreach (var attachment in node.Attachments.Where(x => x.Type == NodeAttachmentType.Mesh))
                    {
                        var nodeMesh = (NodeMeshAttachment)attachment;
                        if (nodeMesh.Mesh.Field14 == 69)
                            nodeMesh.Mesh.Field14 = 0;
                        newMeshNodeAttachments.Add(nodeMesh);
                    }
                    var newAttachments = node.Attachments.Where(x => x.Type != NodeAttachmentType.Mesh).ToList();
                    foreach (var meshattach in newMeshNodeAttachments)
                        newAttachments.Add(meshattach);
                    node.Attachments = newAttachments;
                    outGFS.Model.RootNode.AddChildNode(node);

                    Console.WriteLine($"Added Node \"{node.Name}\" to GFS", ConsoleColor.Green);
                }
                else
                {
                    // If node exists in the original GFS, get mesh attachments
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

                    // Remove meshes with materials marked as "96" in edited GFS, otherwise add meshes marked as "69"
                    List<string> matsToRemove = new();
                    if (false)
                        matsToRemove = new() { "gfdDefaultMat0" };
                    List<NodeMeshAttachment> newMeshAttachments = new();
                    // Add new mesh attachments if marked as "69" in the edited GFS
                    foreach (var attachment in editedMeshes.Where(x => x.Mesh.Field14 == 69))
                    {
                        attachment.Mesh.Field14 = 0;
                        newMeshAttachments.Add(attachment);
                        Console.WriteLine($"Added Mesh Attachment with Material \"{attachment.Mesh.MaterialName}\" to Node \"{node.Name}\"", ConsoleColor.Green);
                    }

                    // Mark meshes with 96 as "removed" in the edited GFS
                    foreach (var attachment in editedMeshes.Where(x => x.Mesh.Field14 == 96))
                    {
                        matsToRemove.Add(attachment.Mesh.MaterialName);
                    }

                    // Add original mesh attachments if they are using a material marked for removal
                    foreach (var mesh in ogMeshAttachments.Where(x => x.Type == NodeAttachmentType.Mesh))
                    {
                        var nodeMesh = (NodeMeshAttachment)mesh;
                        if (!matsToRemove.Any(y => y.Equals(nodeMesh.Mesh.MaterialName)))
                        {
                            newMeshAttachments.Add(nodeMesh);
                            Console.WriteLine($"Marked Mesh Attachment with Material \"{nodeMesh.Mesh.MaterialName}\" for removal in Node \"{node.Name}\"", ConsoleColor.Red);
                        }
                    }

                    // Remove mesh attachments from final node, re-add eligable attachments
                    outNode.Attachments = outNode.Attachments.Where(x => x.Type != NodeAttachmentType.Mesh).ToList();
                    foreach (var mesh in newMeshAttachments)
                        outNode.Attachments.Add(mesh);
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
