using GFDLibrary;
using GFDLibrary.Effects;
using GFDLibrary.Materials;
using GFDLibrary.Models;
using GFDLibrary.Textures;

namespace CostumeUpdater
{
    internal partial class Program
    {
        public static string gDrivePath = @"D:\Google Drive\Modding\Projects\P5R Vinesauce Mod\Models\_PARTY\";
        public static string costumesPath = @"C:\Reloaded\Mods\p5rpc.vinesauce\Costumes\";
        public static string charaModelsPath = @"C:\Reloaded\Mods\p5rpc.vinesauce\Mod Files\Main\_Models\CHARACTER.CPK\MODEL\CHARACTER\";

        static void Main(string[] args)
        {
            
            UpdateMatsAndTexFromSampleModel(
                "D:\\Google Drive\\Modding\\Projects\\P5R Vinesauce Mod\\Models\\_PARTY\\" +
                "1_Vinny_(Joker)\\7_156_Swimsuit_Visions Hoodie\\" +
                "7_156_Swimsuit_Visions Hoodie.GMD");
            
            CopyModelsToOutputFolders();

            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        private static void UpdateMatsAndTexFromSampleModel(string sampleGmdPath)
        {
            ModelPack sampleGMD = Resource.Load<ModelPack>(sampleGmdPath);

            foreach (var character in CharaBatches)
            {
                string gDrive = Path.Combine(gDrivePath, character.GDriveDir);

                if (!Directory.Exists(gDrive))
                    continue;

                foreach (var dir in Directory.GetDirectories(gDrive, "*", SearchOption.TopDirectoryOnly))
                {
                    foreach (var gmd in Directory.GetFiles(dir, "*.GMD", SearchOption.TopDirectoryOnly))
                    {
                        ModelPack modelGMD = Resource.Load<ModelPack>(gmd);
                        bool anyChanges = false;

                        // Update Textures
                        var updatedTextures = modelGMD.Textures.Textures.ToList(); // Create a modifiable list
                        for (int i = 0; i < updatedTextures.Count; i++)
                        {
                            for (int x = 0; x < sampleGMD.Textures.Textures.Count; x++)
                            {
                                if (updatedTextures[i].Name == sampleGMD.Textures.Textures.ToList()[x].Name
                                    && updatedTextures[i] != sampleGMD.Textures.Textures.ToList()[x])
                                {
                                    Console.WriteLine($"Updating Texture {updatedTextures[i].Name} in: {Path.GetFileName(gmd)}");
                                    updatedTextures[i] = sampleGMD.Textures.Textures.ToList()[x]; // Replace the texture
                                    anyChanges = true;
                                }
                            }
                        }

                        modelGMD.Textures.Clear();
                        foreach (var texture in updatedTextures)
                        {
                            modelGMD.Textures.Add(texture); // Add each texture individually
                        }

                        // Update Materials
                        var updatedMaterials = modelGMD.Materials.Materials.ToList(); // Create a modifiable list
                        for (int i = 0; i < updatedMaterials.Count; i++)
                        {
                            for (int x = 0; x < sampleGMD.Materials.Materials.Count; x++)
                            {
                                if (updatedMaterials[i].Name == sampleGMD.Materials.Materials[x].Name
                                    && updatedMaterials[i] != sampleGMD.Materials.Materials[x])
                                {
                                    Console.WriteLine($"Updating Material {updatedMaterials[i].Name} in: {Path.GetFileName(gmd)}");
                                    updatedMaterials[i] = sampleGMD.Materials.Materials[x]; // Replace the material
                                    anyChanges = true;
                                }
                            }
                        }

                        modelGMD.Materials.Clear(); // Clear the existing materials in the MaterialDictionary  
                        foreach (var material in updatedMaterials)
                        {
                            modelGMD.Materials.Add(material); // Add each updated material individually  
                        }

                        if (anyChanges)
                        {
                            Console.WriteLine($"Saving changes to: {gmd}");
                            modelGMD.Save(gmd);
                        }
                    }
                }
            }
        }

        private static void CopyModelsToOutputFolders()
        {
            foreach (var character in CharaBatches)
            {
                string gDrive = Path.Combine(gDrivePath, character.GDriveDir);

                if (!Directory.Exists(gDrive))
                    continue;

                var dirs = Directory.GetDirectories(gDrive, "*", SearchOption.TopDirectoryOnly)
                    .Where(x => character.Outfits.Any(y =>
                        x.Contains("_" + y.Name + "_")));

                foreach (var dir in dirs)
                {
                    Console.WriteLine(dir);
                    if (Directory.GetFiles(dir, "*.GMD", SearchOption.TopDirectoryOnly).Length == 0)
                        continue;

                    var outfit = character.Outfits.First(x => dir.Contains("_" + x.Name + "_"));

                    var gmd = Directory.GetFiles(dir, "*.GMD").FirstOrDefault();

                    ModelPack modelGMD = Resource.Load<ModelPack>(gmd);

                    string costumeDir = Path.Combine(costumesPath, character.Character, outfit.Name);
                    if (!Directory.Exists(costumeDir))
                        Directory.CreateDirectory(costumeDir);
                    string charaModelDir = Path.Combine(charaModelsPath, character.CharaModelDir);

                    // Append attachment to model if it exists
                    if (outfit.Attachment != null)
                    {
                        string attachmentPath = Path.Combine(dir, outfit.Attachment.Item2);
                        if (File.Exists(attachmentPath))
                        {
                            NodeEplAttachment attachmentGMD = new NodeEplAttachment() { Epl = (Epl)Epl.Load(attachmentPath) };
                            modelGMD.Model.Nodes.First(x => x.Name.Equals(outfit.Attachment.Item1)).Attachments.Add(attachmentGMD);
                            Console.WriteLine($"\t[EPL] Added attachment: {outfit.Attachment.Item2}");
                        }
                        else
                            Console.WriteLine($"\t[ERROR] Could not find attachment: {attachmentPath}\r\n\t\tfor: {gmd}");
                    }

                    // Copy model to Costumes Framework folder
                    Console.WriteLine($"Saved {Path.GetFileName(gmd)} to: {costumeDir}");
                    modelGMD.Save(Path.Combine(costumeDir, "costume.gmd"));

                    // Copy model to MODEL/CHARACTER folder
                    foreach (var modelID in outfit.ModelIDs)
                    {
                        modelGMD.Save(Path.Combine(charaModelDir, modelID));
                        Console.WriteLine($"Saved {Path.GetFileName(gmd)} to: {modelID}");
                    }
                }
            }
        }
    }

    public class OutfitBatch
    {
        public List<Outfit> Outfits { get; set; } = new();
        public string Character { get; set; } = "Joker";
        public string GDriveDir { get; set; } = "1_Vinny_(Joker)";
        public string CharaModelDir { get; set; } = "0001";

    }
    public class Outfit
    {
        public string Name { get; set; } = "";
        public string[] ModelIDs { get; set; } = { };
        public bool charaModelDirOnly { get; set; } = false;
        public Tuple<string,string>? Attachment { get; set; } = null;
    }
}
