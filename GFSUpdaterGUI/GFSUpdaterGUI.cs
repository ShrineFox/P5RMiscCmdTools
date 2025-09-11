using GFDFldModelUpdater;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace GFSUpdaterGUI
{
    public partial class GFSUpdaterGUI : Form
    {
        public GFSUpdaterGUI()
        {
            InitializeComponent();
        }

        public static Settings config = new Settings();

        [Serializable]
        public class Settings
        {
            public string OG_GFS { get; set; } = "";
            public string EditedGMD { get; set; } = "";
            public string OutputDir { get; set; } = "";
            public bool GFSMode { get; set; } = false;
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

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            if (gFSModeToolStripMenuItem.Checked)
            {
                GFDFldModelUpdater.Program.Main(new string[]
                {
                    txt_OG_GFS.Text,
                    txt_EditedGMD.Text,
                    Path.Combine(txt_OutputDir.Text, Path.GetFileName(txt_OG_GFS.Text))
                });
            }
            else
            {
                GFDModelUpdater.Program.Main(new string[]
                {
                    txt_OG_GFS.Text,
                    txt_EditedGMD.Text,
                    Path.Combine(txt_OutputDir.Text, Path.GetFileName(txt_EditedGMD.Text))
                });
            }

            MessageBox.Show("Done!");
        }

        private void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void DragDrop_GMD(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileList.Length > 0 && File.Exists(fileList[0]))
            {
                config.EditedGMD = fileList[0];
            }

            ToggleGenerateBtn();
        }

        private void DragDrop_GFS(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileList.Length > 0 && File.Exists(fileList[0]))
            {
                config.OG_GFS = fileList[0];
            }

            ToggleGenerateBtn();
        }

        private void DragDrop_Dir(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileList.Length > 0 && Directory.Exists(fileList[0]))
            {
                config.OutputDir = fileList[0];
            }

            ToggleGenerateBtn();
        }

        private void ToggleGenerateBtn()
        {
            if (!string.IsNullOrEmpty(config.OutputDir))
                txt_OutputDir.Text = config.OutputDir;
            if (!string.IsNullOrEmpty(config.OG_GFS))
                txt_OG_GFS.Text = config.OG_GFS;
            if (!string.IsNullOrEmpty(config.EditedGMD))
                txt_EditedGMD.Text = config.EditedGMD;

            if (txt_EditedGMD.Text != "" && txt_OG_GFS.Text != "" && txt_OutputDir.Text != "")
                btn_GenerateOutput.Enabled = true;
            else
                btn_GenerateOutput.Enabled = false;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json";
            saveFileDialog.Title = "Save a JSON file";
            saveFileDialog.FileName = "GFSUpdaterConfig.json";

            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                SaveJson(saveFileDialog.FileName);
                MessageBox.Show("Saved config to " + saveFileDialog.FileName);
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json";
            openFileDialog.Title = "Select a JSON file";
            openFileDialog.FileName = "GFSUpdaterConfig.json";

            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                LoadJson(openFileDialog.FileName);
                ToggleGenerateBtn();
            }
        }
    }
}
