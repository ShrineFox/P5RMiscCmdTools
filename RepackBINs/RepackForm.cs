using AtlusFileSystemLibrary;
using AtlusFileSystemLibrary.Common.IO;
using AtlusFileSystemLibrary.FileSystems.PAK;
using GFDLibrary.Textures;
using Newtonsoft.Json;
using System.Data;

namespace RepackBINs
{
    public partial class RepackForm : Form
    {
        public Settings config = new Settings();
        string configPath = @"RepackBINsConfig.json";

        public RepackForm()
        {
            InitializeComponent();
            // Load configuration settings from json near exe
            LoadJson(configPath);
            // Add a checkbox for each area listed in the config
            foreach (var area in config.Fields)
                checkedListBox_Areas.Items.Add(area.Name);
            // Update Checkbox States
            if (config.ShrinkNewTextures)
                chk_ShrinkNewTex.Checked = true;
            if (config.ShrinkAllTextures)
                chk_ShrinkAllTex.Checked = true;
            if (config.UseRepackedInput)
                chk_UseRepackedInput.Checked = true;
        }

        public void SaveJson(string jsonPath)
        {
            // Update Checkbox States
            config.ShrinkNewTextures = chk_ShrinkNewTex.Checked;
            config.ShrinkAllTextures = chk_ShrinkAllTex.Checked;
            config.UseRepackedInput = chk_UseRepackedInput.Checked;

            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented));
        }

        public void LoadJson(string jsonPath)
        {
            if (!File.Exists(jsonPath))
                return;

            string jsonText = File.ReadAllText(Path.GetFullPath(jsonPath));
            config = JsonConvert.DeserializeObject<Settings>(jsonText);
        }

        private void Repack_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(config.OriginalBINDir))
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog() { UseDescriptionForTitle = true, Description = "Choose Unedited, Original .BINs Folder" };
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    config.OriginalBINDir = folderDialog.SelectedPath;
                    SaveJson(configPath);
                }
                else
                    return;
            }

            foreach (object itemChecked in checkedListBox_Areas.CheckedItems)
            {
                try
                {
                    Field selectedField = config.Fields.First(x => x.Name == itemChecked.ToString());

                    string[] dirsToProcess;

                    if (selectedField.Name == checkedListBox_Areas.Items[0].ToString())
                        dirsToProcess = Directory.GetDirectories(config.LooseBINDir).Where(x =>
                            Convert.ToInt32(Path.GetFileName(x).Split('_').First().Replace("TEX", "").Replace("tex", "")) < 50).ToArray();
                    else
                        dirsToProcess = Directory.GetDirectories(config.LooseBINDir).Where(x =>
                        selectedField.Ids.Any(y =>
                            Convert.ToInt32(Path.GetFileName(x).Split('_').First().Replace("TEX", "").Replace("tex", "")).Equals(y))).ToArray();

                    foreach (var looseBIN in dirsToProcess)
                    {
                        PAKFileSystem pak = new PAKFileSystem();

                        string matchingOriginalBINFile = Path.Combine(config.OriginalBINDir, Path.GetFileName(looseBIN.Replace(".bin", "").Replace(".BIN", ""))) + ".BIN";
                        string matchingRepackedBINFile = Path.Combine(config.RepackedBINDir, Path.GetFileName(looseBIN.Replace(".bin", "").Replace(".BIN", ""))) + ".BIN";
                        // Use repacked BIN if setting is on and repacked BIN exists, saves time if dds is already shrinked
                        if (config.UseRepackedInput && File.Exists(matchingRepackedBINFile))
                            matchingOriginalBINFile = matchingRepackedBINFile;

                        if (PAKFileSystem.TryOpen(matchingOriginalBINFile, out pak))
                        {
                            foreach (var looseBinDds in Directory.GetFiles(looseBIN))
                            {
                                if (chk_ShrinkNewTex.Checked)
                                {
                                    Bitmap bmp = ConvertDDSToBitmap(File.ReadAllBytes(looseBinDds));
                                    
                                    if (bmp.Width > 512 && bmp.Height > 512)
                                    {
                                        Bitmap scaledBmp = ScaleBitmapByHalf(bmp);
                                        
                                        string tempPath = Path.Combine("Temp", Path.GetFileName(looseBinDds));
                                        Console.WriteLine($"\tShrinking New Texture: {Path.GetFileName(looseBinDds)}");
                                        var tex = TextureEncoder.Encode("temp.dds", TextureFormat.DDS, scaledBmp);
                                        MemoryStream ms = new MemoryStream(tex.Data);
                                            pak.AddFile(Path.GetFileName(looseBinDds), ms, false, ConflictPolicy.Replace);
                                        
                                    }
                                }
                                else
                                {
                                    pak.AddFile(Path.GetFileName(looseBinDds), looseBinDds, ConflictPolicy.Replace);
                                    Console.WriteLine($"\tReplacing Texture: {Path.GetFileName(looseBinDds)}");
                                }
                            }

                            if (chk_ShrinkAllTex.Checked)
                            {
                                foreach (var pakDds in pak.EnumerateFiles().Where(x => x.ToLower().EndsWith(".dds")))
                                {
                                    string tempPath = Path.Combine("Temp", Path.GetFileName(pakDds));

                                    Directory.CreateDirectory("Temp");
                                    var ms = new MemoryStream();
                                    using (var inputStream = pak.OpenFile(pakDds))
                                    {
                                        inputStream.CopyTo(ms);
                                    }

                                    Bitmap bmp = ConvertDDSToBitmap(ms.ToArray());
                                        
                                    if (bmp.Width > 512 && bmp.Height > 512)
                                    {
                                        Bitmap scaledBmp = ScaleBitmapByHalf(bmp);

                                        Console.WriteLine($"\tShrinking Existing Texture: {Path.GetFileName(pakDds)}");
                                        var tex = TextureEncoder.Encode("temp.dds", TextureFormat.DDS, scaledBmp);
                                        var ms2 = new MemoryStream(tex.Data);
                                        pak.AddFile(Path.GetFileName(pakDds), ms2, false, ConflictPolicy.Replace);
                                            
                                    }
                                    
                                }
                            }

                            string outputPath = Path.Combine(config.RepackedBINDir, Path.GetFileName(matchingOriginalBINFile));
                            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                            pak.Save(outputPath);
                            Console.WriteLine($"Repacked {outputPath}");
                        }
                        else
                            Console.WriteLine($"Failed to open original BIN: {matchingOriginalBINFile}");
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }

            Console.WriteLine($"\nDone repacking BINs!");

            if (Directory.Exists("Temp"))
                Directory.Delete("Temp", true);
        }

        private Bitmap ScaleBitmapByHalf(Bitmap bmp)
        {
            return new Bitmap(bmp, new Size((int)Math.Ceiling((double)bmp.Width / 2), (int)Math.Ceiling(((double)bmp.Height / 2))));
        }

        private Bitmap ConvertDDSToBitmap(byte[] ddsBytes)
        {
            var texture = new Texture() { Data = ddsBytes, Format = TextureFormat.DDS };
            return TextureDecoder.Decode(texture);
        }

        /// <summary>
        /// Waits up to 20 seconds for a file to exist and become available to open.
        /// </summary>
        /// <param name="fullPath">The path to the file to wait for.</param>
        /// <returns></returns>
        public static FileStream WaitForFile(string fullPath,
            FileMode mode = FileMode.Open,
            FileAccess access = FileAccess.ReadWrite,
            FileShare share = FileShare.None)
        {
            for (int numTries = 0; numTries < 10; numTries++)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fullPath, mode, access, share);
                    return fs;
                }
                catch (IOException)
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                    Thread.Sleep(2000);
                }
            }
            return null;
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            bool check = true;
            if (checkedListBox_Areas.GetItemCheckState(0) == CheckState.Checked)
                check = false;

            for (int i = 0; i < checkedListBox_Areas.Items.Count; i++)
            {
                checkedListBox_Areas.SetItemChecked(i, check);
            }
        }
    }

    public class Field
    {
        public string Name { get; set; } = "";
        public List<int> Ids { get; set; } = new List<int>();
    }

    public class Settings
    {
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<Field> Fields { get; set; } = new List<Field>()
        {
            new Field() { Name = "Misc (Overworld)", Ids = { -1 } },
            new Field() { Name = "Mementos", Ids = { 90, 91, 92, 93, 94, 95, 190, 191, 192, 193, 194, 195, 291, 292, 293, 294 } },
            new Field() { Name = "Castle", Ids = { 51, 52, 151, 152, 251, 252 } },
            new Field() { Name = "Museum", Ids = { 53, 153, 253 } },
            new Field() { Name = "Bank", Ids = { 54, 154, 254 } },
            new Field() { Name = "Pyramid", Ids = { 55, 155, 255 } },
            new Field() { Name = "Spaceship", Ids = { 56, 156, 256 } },
            new Field() { Name = "Casino", Ids = { 50, 57, 157, 257 } },
            new Field() { Name = "Cruise Ship", Ids = { 59, 159, 259 } },
            new Field() { Name = "Qliphoth Tree", Ids = { 60, 160, 260 } },
            new Field() { Name = "Mementos Depths", Ids = { 61, 161, 261 } },
            new Field() { Name = "Laboratory", Ids = { 62, 162, 262 } }
        };
        public bool ShrinkNewTextures { get; set; } = true;
        public bool ShrinkAllTextures { get; set; } = false;
        public bool UseRepackedInput { get; set; } = true;
        public string OriginalBINDir { get; set; } = "";
        public string LooseBINDir { get; set; } = @"LooseBINs\MODEL\FIELD_TEX\TEXTURES";
        public string RepackedBINDir { get; set; } = @"RepackedBINs\TEX_WIP.CPK\MODEL\FIELD_TEX\TEXTURES";
    }

}
