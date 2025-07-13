using GFDLibrary;

namespace GFDSearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach(var file in Directory.GetFiles(args[0], "*", SearchOption.TopDirectoryOnly)
                .Where(x => x.ToLower().EndsWith(".gmd") || x.ToLower().EndsWith(".gfs"))
                )
            {
                ModelPack gmd = Resource.Load<ModelPack>(file);

                foreach (var node in gmd.Model.Nodes)
                {
                    if (node.Name.ToLower().Contains(args[1]))
                    {
                        Console.WriteLine($"Found GFD in {file} - {node.Name}");
                        return;
                    }
                }
            }
        }
    }
}
