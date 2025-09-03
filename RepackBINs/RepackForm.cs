using AtlusFileSystemLibrary;
using AtlusFileSystemLibrary.Common.IO;
using AtlusFileSystemLibrary.FileSystems.PAK;
using GFDLibrary.Textures;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Reflection;

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
            try
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
                        string matchingOriginalBINFile = Path.Combine(config.OriginalBINDir, Path.GetFileName(looseBIN.Replace(".bin", "").Replace(".BIN", ""))) + ".BIN";
                        string matchingRepackedBINFile = Path.Combine(config.RepackedBINDir, Path.GetFileName(looseBIN.Replace(".bin", "").Replace(".BIN", ""))) + ".BIN";
                        if (config.UseRepackedInput && File.Exists(matchingRepackedBINFile))
                        {
                            matchingOriginalBINFile = matchingRepackedBINFile;
                        }

                        PAKFileSystem pak = new PAKFileSystem();
                        {
                            if (!PAKFileSystem.TryOpen(matchingOriginalBINFile, out pak))
                            {
                                Console.WriteLine($"Failed to open original BIN: {matchingOriginalBINFile}");
                                continue;
                            }

                            foreach (var looseBinDds in Directory.GetFiles(looseBIN))
                            {
                                if (chk_ShrinkNewTex.Checked)
                                {
                                    Bitmap bmp = ConvertDDSToBitmap(File.ReadAllBytes(looseBinDds));
                                    while (bmp.Width > 512 || bmp.Height > 512)
                                    {
                                        bmp = ScaleBitmapByHalf(bmp);
                                    }

                                    Console.WriteLine($"\tShrinking New Texture: {Path.GetFileName(looseBinDds)}");
                                    var tex = TextureEncoder.Encode("temp.dds", TextureFormat.DDS, bmp);
                                    MemoryStream ms = new MemoryStream(tex.Data);
                                    pak.AddFile(Path.GetFileName(looseBinDds), ms, true, ConflictPolicy.Replace);
                                    
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
                                    MemoryStream ms = new MemoryStream();
                                    {
                                        using (var inputStream = pak.OpenFile(pakDds))
                                        {
                                            inputStream.CopyTo(ms);
                                        }
                                        Bitmap bmp = ConvertDDSToBitmap(ms.ToArray());
                                        while (bmp.Width > 512 || bmp.Height > 512)
                                        {
                                            bmp = ScaleBitmapByHalf(bmp);
                                        }
                                        Console.WriteLine($"\tShrinking Existing Texture: {Path.GetFileName(pakDds)}");
                                        var tex = TextureEncoder.Encode("temp.dds", TextureFormat.DDS, bmp);
                                        var ms2 = new MemoryStream(tex.Data);
                                        pak.AddFile(Path.GetFileName(pakDds), ms2, true, ConflictPolicy.Replace);
                                        
                                    }
                                    //ms.Dispose();
                                }
                            }

                            string outputPath = Path.Combine(config.RepackedBINDir, Path.GetFileName(matchingOriginalBINFile));
                            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                            pak.Save(outputPath + "_");
                            Console.WriteLine($"Repacked {outputPath}");
                        }
                    }
                }

                Console.WriteLine($"\nDone repacking BINs!");

                Process.Start("BinCleanup.exe");
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
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

    public static class ObjectExtensions
    {
        private static readonly MethodInfo CloneMethod = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool IsPrimitive(this Type type)
        {
            if (type == typeof(String)) return true;
            return (type.IsValueType & type.IsPrimitive);
        }

        /// <summary>
        /// Create a deep copy of an object.
        /// </summary>
        /// <param name="originalObject">The object to copy.</param>
        /// <returns></returns>
        public static Object Copy(this Object originalObject)
        {
            return InternalCopy(originalObject, new Dictionary<Object, Object>(new ReferenceEqualityComparer()));
        }
        private static Object InternalCopy(Object originalObject, IDictionary<Object, Object> visited)
        {
            if (originalObject == null) return null;
            var typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect)) return originalObject;
            if (visited.ContainsKey(originalObject)) return visited[originalObject];
            if (typeof(Delegate).IsAssignableFrom(typeToReflect)) return null;
            var cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray)
            {
                var arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false)
                {
                    Array clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }

            }
            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null)
            {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }

        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (filter != null && filter(fieldInfo) == false) continue;
                if (IsPrimitive(fieldInfo.FieldType)) continue;
                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }
        public static T Copy<T>(this T original)
        {
            return (T)Copy((Object)original);
        }
    }

    public class ReferenceEqualityComparer : EqualityComparer<Object>
    {
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }
        public override int GetHashCode(object obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }

    public static class ArrayExtensions
    {
        public static void ForEach(this Array array, Action<Array, int[]> action)
        {
            if (array.LongLength == 0) return;
            ArrayTraverse walker = new ArrayTraverse(array);
            do action(array, walker.Position);
            while (walker.Step());
        }
    }

    internal class ArrayTraverse
    {
        public int[] Position;
        private int[] maxLengths;

        public ArrayTraverse(Array array)
        {
            maxLengths = new int[array.Rank];
            for (int i = 0; i < array.Rank; ++i)
            {
                maxLengths[i] = array.GetLength(i) - 1;
            }
            Position = new int[array.Rank];
        }

        public bool Step()
        {
            for (int i = 0; i < Position.Length; ++i)
            {
                if (Position[i] < maxLengths[i])
                {
                    Position[i]++;
                    for (int j = 0; j < i; j++)
                    {
                        Position[j] = 0;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}

