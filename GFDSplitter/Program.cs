using GFDLibrary;
using GFDStudio.FormatModules;
using ShrineFox.IO;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace GFDSplitter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var modelPack = ModuleImportUtilities.ImportFile<ModelPack>(args[0]);

            foreach(var node in modelPack.Model.Nodes.Where(x => x.Name.StartsWith("IT8000_1")))
            {
                string newName = node.Name.Replace("IT8000_1", "");
                int digitPlace = int.Parse(newName[0].ToString());
                int digit = int.Parse(newName[1].ToString());

                var gmdCopy = modelPack.Copy();

                for (int i = 0; i < gmdCopy.Model.Nodes.Count(); i++)
                {
                    //try
                    {
                        if (gmdCopy.Model.Nodes.ToList()[i].Name.Equals($"IT8000_1{digitPlace}{digit}"))
                        {
                            gmdCopy.Model.Nodes.ToList()[i].Name = $"{digitPlace}_{digit}";
                        }
                    }
                    //catch { }
                }

                for (int x = 0; x < 10; x++)
                for (int i = 0; i < gmdCopy.Model.RootNode.Children.Count(); i++)
                {
                    //try
                    {
                        if (!gmdCopy.Model.RootNode.Children.ToList()[i].Name.Equals($"{digitPlace}_{digit}")
                            && !gmdCopy.Model.RootNode.Children.ToList()[i].Children.Any(x => x.Name.Equals($"{digitPlace}_{digit}")))
                        {
                            gmdCopy.Model.RootNode.RemoveChildNode(gmdCopy.Model.RootNode.Children.ToList()[i]);
                        }
                    }
                    //catch { }
                }

                /*
                for (int i = 0; i < gmdCopy.Model.Nodes.Count(); i++)
                {
                    //try
                    {
                        if (gmdCopy.Model.Nodes.ToList()[i].Parent != null && !gmdCopy.Model.Nodes.ToList()[i].Name.Equals($"{digit}_{digitPlace}"))
                        {
                            gmdCopy.Model.Nodes.ToList()[i].Parent.RemoveChildNode(gmdCopy.Model.Nodes.ToList()[i]);
                        }
                    }
                    //catch { }
                }
                */

                gmdCopy.Save($".\\IT8000_1{digitPlace}{digit}.GMD");
            }
            
        }

    }
}
