using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostumeUpdater
{
    internal partial class Program
    {
        public static List<OutfitBatch> CharaBatches = new List<OutfitBatch>
        {
        new()
        {
            Character = "Joker",
            GDriveDir = "1_Vinny_(Joker)",
            CharaModelDir = "0001",
            Outfits = new List<Outfit>
                {
                    new Outfit
                    {
                        Name = "Phantom Suit", // Streamer Attire
                        ModelIDs = new[] { "C0001_051_00.GMD", "C0001_052_00.GMD", "C0001_103_00.GMD", "C0001_119_00.GMD", "C0001_171_00.GMD" },
                    },
                    new Outfit
                    {
                        Name = "Summer Uniform", // Sunday Best
                        Attachment = new Tuple<string, string>("Bip01", "Corruption_RootNodeAttachment.epl")
                    },
                    new Outfit
                    {
                        Name = "Winter Uniform", // V-Dub's Drug Rugs
                        Attachment = new Tuple<string, string>("b d tooth.001", "Smoke_b b tooth01_Attachment.epl")
                    },
                    new Outfit
                    {
                        Name = "Summer Clothes", // What Could Go Wrong
                    },
                    new Outfit
                    {
                        Name = "Winter Clothes", // Another Light Hoodie
                    },
                    new Outfit
                    {
                        Name = "Tracksuit", // Realign Hoodie
                    },
                    new Outfit
                    {
                        Name = "Swimsuit", // Visions Hoodie
                    },
                    new Outfit
                    {
                        Name = "Loungewear", // Afterthoughts Hoodie
                    },
                    new Outfit
                    {
                        Name = "Summer Clothes", // What Could Go Wrong
                    },
                    new Outfit
                    {
                        Name = "Gekkoukan High", // Red Vox Ensemble
                        ModelIDs = new[] { "C0001_004_00.GMD" },
                    },
                    new Outfit
                    {
                        Name = "Yasogami High", // Blood Bagel Clown
                    },
                    new Outfit
                    {
                        Name = "St. Hermelin High", // Low Poly Vinny
                    },
                    new Outfit
                    {
                        Name = "Seven Sisters High", // V-Dub Space Cadet
                    },
                    new Outfit
                    {
                        Name = "Vincent's Outfit", // Forgetter
                    },
                    new Outfit
                    {
                        Name = "Karukozaka High", // Vinemon Trainer
                    },
                    new Outfit
                    {
                        Name = "Butler Suit", // Gormless Hoarder
                        ModelIDs = new[] { "C0001_072_00.GMD", "C0001_061_00.GMD" },
                    },
                    new Outfit
                    {
                        Name = "Christmas Outfit", // Hoarder Tracksuit
                    },
                    new Outfit
                    {
                        Name = "Dancewear", // Tomodachi Look-Alike
                    },
                    new Outfit
                    {
                        Name = "Shadow Ops Uniform", // Vine's Thief Tunic
                    },
                    new Outfit
                    {
                        Name = "Samurai Garb", // VineRealms Standee
                    },
                    new Outfit
                    {
                        Name = "Yumizuki High", // Shujin Winter Uniform
                        ModelIDs = new[] { "C0001_002_00.GMD", "C0001_106_00.GMD" },
                    },
                    new Outfit
                    {
                        Name = "Starlight Outfit", // Shujin Summer Uniform
                        ModelIDs = new[] { "C0001_001_00.GMD" },
                    },
                    new Outfit
                    {
                        Name = "Featherman Suit", // VineRider
                    },
                    new Outfit
                    {
                        Name = "Beta Winter Uniform", // Early Vinny Model
                        ModelIDs = new[] { "C0001_181_00.GMD" },
                    },
                    new Outfit
                    {
                        Name = "Hometown Clothes", // Past Vinny
                        ModelIDs = new[] { "C0001_071_00.GMD" },
                    }
                }
            }
        };
    }
}
