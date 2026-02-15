namespace P5RCreditsConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args[0].EndsWith(".bin"))
            {
                string creditsBinPath = args[0];
                string outputTxtPath = ".\\out.txt";

                using (FileStream fs = new FileStream(creditsBinPath, FileMode.Open))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    CreditsBin creditsBin = new CreditsBin();
                    creditsBin.Field00 = br.ReadUInt32();
                    creditsBin.RelocationTableOffset = br.ReadUInt32();
                    creditsBin.RelocationTableSize = br.ReadUInt32();
                    creditsBin.StringCount = br.ReadUInt32();
                    creditsBin.StringLengthTableOffset = br.ReadUInt32();
                    creditsBin.StringBufferOffset = br.ReadUInt32();
                    creditsBin.StringBufferSize = br.ReadUInt32();
                    fs.Seek(creditsBin.StringLengthTableOffset, SeekOrigin.Begin);
                    for (int i = 0; i < creditsBin.StringCount; i++)
                    {
                        creditsBin.StringLengths.Add(br.ReadByte());
                    }
                    fs.Seek(creditsBin.StringBufferOffset, SeekOrigin.Begin);
                    for (int i = 0; i < creditsBin.StringCount; i++)
                    {
                        byte[] stringBytes = br.ReadBytes(creditsBin.StringLengths[i]);
                        string str = System.Text.Encoding.UTF8.GetString(stringBytes);
                        creditsBin.Strings.Add(str);
                    }
                    fs.Seek(creditsBin.RelocationTableOffset, SeekOrigin.Begin);
                    for (int i = 0; i < creditsBin.RelocationTableSize; i++)
                    {
                        creditsBin.RelocationTable.Add(br.ReadByte());
                    }
                    File.WriteAllLines(outputTxtPath, creditsBin.Strings);
                }
            }
            else
            {
                string inputTxtPath = args[0];
                string outputBinPath = ".\\out.bin";
                CreditsBin creditsBin = new CreditsBin();
                string[] lines = File.ReadAllLines(inputTxtPath);
                creditsBin.StringCount = (uint)lines.Length;
                foreach (string line in lines)
                {
                    byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(line);
                    if (stringBytes.TakeLast(2).ToArray() != new byte[] { 0x0D, 0x0A })
                    {
                        stringBytes = stringBytes.Concat(new byte[] { 0x09, 0x00, 0x0D, 0x0A }).ToArray();
                    }

                    byte length = (byte)stringBytes.Length;
                    creditsBin.StringLengths.Add(length);
                    creditsBin.Strings.Add(line);
                }
                using (FileStream fs = new FileStream(outputBinPath, FileMode.Create))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(creditsBin.Field00);
                    bw.Write(creditsBin.RelocationTableOffset);
                    bw.Write(creditsBin.RelocationTableSize);
                    bw.Write(creditsBin.StringCount);
                    bw.Write(creditsBin.StringLengthTableOffset);
                    bw.Write(creditsBin.StringBufferOffset);
                    bw.Write(creditsBin.StringBufferSize);
                    foreach (byte length in creditsBin.StringLengths)
                    {
                        bw.Write(length);
                    }
                    foreach (string str in creditsBin.Strings)
                    {
                        byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(str);
                        if (stringBytes.TakeLast(2).ToArray() != new byte[] { 0x0D, 0x0A })
                        {
                            stringBytes = stringBytes.Concat(new byte[] { 0x09, 0x00, 0x0D, 0x0A }).ToArray();
                        }
                        bw.Write(stringBytes);
                    }
                    foreach (byte b in creditsBin.RelocationTable)
                    {
                        bw.Write(b);
                    }
                }
            }
            

        }

        public class CreditsBin
        {
            public uint Field00 { get; set; } = 1527;
            public uint RelocationTableOffset { get; set; } = 27312;
            public uint RelocationTableSize { get; set; } = 1578;
            public uint StringCount { get; set; } = 1320;
            public uint StringLengthTableOffset { get; set; } = 28;
            public uint StringBufferOffset { get; set; } = 1348;
            public uint StringBufferSize { get; set; } = 25964;
            public List<Byte> StringLengths { get; set; } = new List<Byte>();
            public List<string> Strings { get; set; } = new List<string>();
            public List<Byte> RelocationTable { get; set; } = new List<Byte>();

        }
    }
}
