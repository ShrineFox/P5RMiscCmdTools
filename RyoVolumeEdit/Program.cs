namespace RyoVolumeEdit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var yamlPath in Directory.GetFiles(args[0], "*.yaml", SearchOption.AllDirectories))
            {
                string[] yamlLines = File.ReadAllLines(yamlPath);
                List<string> newYamlLines = new List<string>();
                foreach (var line in yamlLines.Where(x => !x.StartsWith("volume:") && !x.StartsWith("use_player_volume:")))
                    newYamlLines.Add(line);
                File.WriteAllLines(yamlPath, newYamlLines);
            }
        }
    }
}
