using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AtlusFileSystemLibrary.FileSystems.PAK;
using AtlusFileSystemLibrary;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace RepackBINs
{
    public partial class RepackForm : Form
    {
        public Settings config = new Settings();
        string configPath = @"RepackBINsConfig.json";

        public RepackForm()
        {
            InitializeComponent();
            LoadJson(configPath);
            foreach (var area in config.Fields)
                checkedListBox_Areas.Items.Add(area.Name);
        }

        public void SaveJson(string jsonPath)
        {
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
                        if (PAKFileSystem.TryOpen(matchingOriginalBINFile, out pak))
                        {
                            foreach (var looseBinDds in Directory.GetFiles(looseBIN))
                            {
                                pak.AddFile(Path.GetFileName(looseBinDds), looseBinDds, ConflictPolicy.Replace);
                            }
                            string outputPath = Path.Combine(config.RepackedBINDir, Path.GetFileName(matchingOriginalBINFile));
                            Console.WriteLine($"Repacking {outputPath}");
                            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                            pak.Save(outputPath);
                        }
                        else
                            Console.WriteLine($"Failed to open original BIN: {matchingOriginalBINFile}");
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }

            Console.WriteLine($"\nDone repacking BINs!");
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
        public string OriginalBINDir { get; set; } = "";
        public string LooseBINDir { get; set; } = @"LooseBINs\MODEL\FIELD_TEX\TEXTURES";
        public string RepackedBINDir { get; set; } = @"RepackedBINs\TEX_WIP.CPK\MODEL\FIELD_TEX\TEXTURES";
    }

}
