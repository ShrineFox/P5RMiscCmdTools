using Newtonsoft.Json;

namespace GFDMatFinder
{
    public partial class MatFinder : Form
    {
        public static Dump dump = new Dump();

        public MatFinder()
        {
            InitializeComponent();

            CreateCheckboxes();
            //GetMatsFromDumpTxtDir("D:\\CPK\\x_MatDumps");
            //SaveJson("P5RMatDump.json");
            LoadJson("P5RMatDump.json");
        }

        private void CreateCheckboxes()
        {
            foreach (var flag in GeometryFlags)
                checkedListBox_GeoFlags.Items.Add(flag);

            foreach (var flag in VertexAttributeFlags)
                checkedListBox_VertFlags.Items.Add(flag);

            foreach (var flag in MaterialFlags)
                checkedListBox_MatFlags.Items.Add(flag);
        }

        private void FindMats_Click(object sender, EventArgs e)
        {
            textBox_MatsList.Text = "";

            var checkedGeoFlags = checkedListBox_GeoFlags.CheckedItems.Cast<string>().ToList();
            var checkedVertFlags = checkedListBox_VertFlags.CheckedItems.Cast<string>().ToList();
            var checkedMatFlags = checkedListBox_MatFlags.CheckedItems.Cast<string>().ToList();

            foreach (var mat in dump.Materials.Where(mat =>
                checkedGeoFlags.All(flag => mat.GeometryFlags.Contains(flag)) &&
                checkedVertFlags.All(flag => mat.VertexAttributeFlags.Contains(flag)) &&
                checkedMatFlags.All(flag => mat.Flags.Contains(flag)) &&
                (!chk_IncludeOnlySelectedGeoFlags.Checked || mat.GeometryFlags.All(flag => checkedGeoFlags.Contains(flag))) &&
                (!chk_IncludeOnlySelectedVertFlags.Checked || mat.VertexAttributeFlags.All(flag => checkedVertFlags.Contains(flag))) &&
                (!chk_IncludeOnlySelectedMatFlags.Checked || mat.Flags.All(flag => checkedMatFlags.Contains(flag)))))
            {
                textBox_MatsList.Text += $"{mat.Name} (from {mat.ModelFile})\r\n";
            }

        }

        public static List<string> GeometryFlags = new List<string>() { "HasVertexWeights", "HasMaterial", "HasTriangles", "HasBoundingBox", "HasBoundingSphere", "Bit5", "HasMorphTargets", "Bit7", "Bit8", "Bit9", "Bit10", "Bit11", "Bit12", "Bit13", "Bit14", "Bit15", "Bit16", "Bit17", "Bit18", "Bit19", "Bit20", "Bit21", "Bit22", "Bit23", "Bit24", "Bit25", "Bit26", "Bit27", "Bit28", "Bit29", "Bit30", "Bit31" };

        public static List<string> VertexAttributeFlags = new List<string>() { "Position", "Bit2", "Bit3", "Normal", "Color0", "Bit6", "TexCoord0", "TexCoord1", "TexCoord2", "Color1", "Bit12", "Bit13", "Bit14", "Bit15", "Bit16", "Bit17", "Bit18", "Bit19", "Bit20", "Bit21", "Bit22", "Bit23", "Bit24", "Bit25", "Bit26", "Bit27", "Tangent", "Binormal", "Color2", "Bit31" };

        public static List<string> MaterialFlags = new List<string>() {"HasAmbientColor","HasDiffuseColor","HasSpecularColor","Transparency","HasVertexColors","ApplyFog","Diffusitivity","HasUVAnimation","HasEmissiveColor","HasReflection","EnableShadow","EnableLight","RenderWireframe","AlphaTest","ReceiveShadow","CastShadow","HasAttributes","HasOutline","SpecularInNormalMap","ReflectionCaster","HasDiffuseMap","HasNormalMap","HasSpecularMap","HasReflectionMap","HasHighlightMap","HasGlowMap","HasNightMap","HasDetailMap","HasShadowMap","HasTextureMap10","ExtraDistortion","Bit31","Bloom","ShadowMapAdd","ShadowMapMultiply","DisableHDR","DisableDeferred","DisableOutline","OpaqueAlpha1","LerpVertexColor","ReflectionMapAdd","Grayscale","DisableFog","Bit11","Bit12","Bit13","Bit14","Bit15" };

        private void GetMatsFromDumpTxtDir(string dumpTxtDir)
        {
            foreach (var txtFile in Directory.GetFiles(dumpTxtDir, "*.txt", SearchOption.TopDirectoryOnly))
            {
                var lines = File.ReadAllLines(txtFile);
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i].StartsWith("# D:"))
                    {
                        Mat mat = new Mat()
                        {
                            ModelFile = lines[i].Replace("#", "").Trim(),
                            Name = lines[i + 1].Replace("#", "").Trim()
                        };

                        foreach (var flag in lines[i + 2].Split(','))
                            mat.Flags.Add(flag.Replace("Flags: ", "").Trim());

                        foreach (var flag in lines[i + 3].Split(','))
                            mat.GeometryFlags.Add(flag.Replace("GeometryFlags: ", "").Trim());

                        foreach (var flag in lines[i + 4].Split(','))
                            mat.VertexAttributeFlags.Add(flag.Replace("VertexAttributeFlags: ", "").Trim());

                        dump.Materials.Add(mat);
                    }
                }
            }
        }


        public void SaveJson(string jsonPath)
        {
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(dump, Newtonsoft.Json.Formatting.Indented));
        }

        public void LoadJson(string jsonPath)
        {
            if (!File.Exists(jsonPath))
                return;

            string jsonText = File.ReadAllText(Path.GetFullPath(jsonPath));
            dump = JsonConvert.DeserializeObject<Dump>(jsonText);
        }
    }

    public class Dump
    {
        public List<Mat> Materials { get; set; } = new List<Mat>();
    }

    public class Mat
    {
        public string Name { get; set; } = "";
        public string ModelFile { get; set; } = "";

        public List<string> Flags { get; set; } = new List<string>();

        public List<string> GeometryFlags { get; set; } = new List<string>();

        public List<string> VertexAttributeFlags { get; set; } = new List<string>();
    }
}
