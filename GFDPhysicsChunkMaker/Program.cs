using System.Formats.Tar;

namespace GFDPhysicsChunkMaker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ymlLines = File.ReadAllLines(args[0]);
            List<BoneEntry> boneEntries = new List<BoneEntry>();
            List<PairEntry> pairEntries = new List<PairEntry>();
            string list2 = "";

            for (int i = 0; i < ymlLines.Length; i++)
            {
                if (!ymlLines[i].StartsWith("-") && !ymlLines[i].StartsWith("E"))
                    continue;

                if (ymlLines[i].StartsWith("- RestoringForce: "))
                {
                    BoneEntry bone = new BoneEntry();
                    bone.RestoringForce = Convert.ToSingle(ymlLines[i].Replace("- RestoringForce: ", ""));
                    bone.WindRate = Convert.ToSingle(ymlLines[i + 1].Replace("  WindRate: ", ""));
                    bone.Mass = Convert.ToSingle(ymlLines[i + 2].Replace("  Mass: ", ""));
                    bone.SphereRadius = Convert.ToSingle(ymlLines[i + 3].Replace("  SphereRadius: ", ""));
                    bone.NodeName = ymlLines[i + 4].Replace("  NodeName: ", "");
                    boneEntries.Add(bone);
                    continue;
                }

                /*
                if (ymlLines[i].StartsWith("Entry2List"))
                {
                    int x = i;
                    while (!ymlLines[x].Contains("Entry3List"))
                    {
                        list2 += ymlLines[x] + "\r\n";
                    }
                    continue;
                }*/

                if (ymlLines[i].StartsWith("- LengthSq: "))
                {
                    PairEntry pair = new PairEntry();
                    pair.LengthSq = Convert.ToSingle(ymlLines[i].Replace("- LengthSq: ", ""));
                    pair.AngularLimit = Convert.ToSingle(ymlLines[i + 1].Replace("  AngularLimit: ", ""));
                    pair.ChainThickness = Convert.ToSingle(ymlLines[i + 2].Replace("  ChainThickness: ", ""));
                    pair.ParentBoneIndex = Convert.ToInt32(ymlLines[i + 3].Replace("  ParentBoneIndex: ", ""));
                    pair.ChildBoneIndex = Convert.ToInt32(ymlLines[i + 4].Replace("  ChildBoneIndex: ", ""));
                    pairEntries.Add(pair);
                    continue;
                }
            }

            string outputTsv = "Bone Index\tBone Name\t0 SphereRadius\t4 Mass\t8 RestoringForce\tC WindRate\r\n";
            int b = 0;
            foreach(var bone in boneEntries)
            {
                outputTsv += $"{b}\t{bone.NodeName}\t{bone.SphereRadius}\t{bone.Mass}\t{bone.RestoringForce}\t{bone.WindRate}\r\n";
                b++;
            }
            outputTsv += "Link Parent\tLink Child\t\"Mass\" LengthSq \tUnk4 AngularLimit\t\"Radius\" ChainThickness \r\n";
            foreach (var pair in pairEntries)
            {
                outputTsv += $"{pair.ParentBoneIndex}\t{pair.ChildBoneIndex}\t{pair.LengthSq}\t{pair.AngularLimit}\t{pair.ChainThickness}\r\n";
            }
            File.WriteAllText("output.tsv", outputTsv);
        }
    }

    public class PairEntry
    {
        public float LengthSq { get; set; } = 0f;
        public float AngularLimit { get; set; } = 0f;
        public float ChainThickness { get; set; } = 0f;
        public int ParentBoneIndex { get; set; } = 0;
        public int ChildBoneIndex { get; set; } = 0;
    }

    public class BoneEntry
    {
        public float SphereRadius { get; set; } = 0f;
        public float Mass { get; set; } = 0f;
        public float RestoringForce { get; set; } = 0f;
        public float WindRate { get; set; } = 0f;
        public string NodeName { get; set; } = "";
    }
}
