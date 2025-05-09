namespace CueSheetGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string str = "";
            for(int i = 0; i < 999; i++)
            {
                /*
                str += $"/Vinesauce,{i.ToString("D5")}_streaming,Cue,,\"voice \",{i},,,,,SynthSequential,\r\n" +
                        $"/Vinesauce/{i.ToString("D5")}_streaming,{i.ToString("D5")}_streaming,Track,,,,,,#2878C8,,,\r\n" +
                        $"/Vinesauce/{i.ToString("D5")}_streaming/{i.ToString("D5")}_streaming,{i.ToString("D5")}_streaming.wav,Waveform,,,,,{i.ToString("D5")}_streaming.wav,,,,\r\n";
                */
                //workunits .materialinfo
                //str += $"<Orca OrcaName=\"{i.ToString("D5")}_streaming.wav\" Channels=\"2\" EncodeFileSize=\"25690\" EncodeType=\"ADX\" LastLoadSrcFileTimeStamp=\"2025/05/06 20:08:50\" ReloopEnd=\"22766\" ResamplingRate=\"44100\" SampleFrames=\"22766\" SamplingRate=\"44100\" SourceFileSize=\"91208\" StreamType=\"Stream\" WaveImageCacheWriteTime=\"2025/05/06 20:08:50\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoWaveform\" />\r\n";
                File.Copy(@"C:\Users\Ryan\Downloads\wav.wav", $"./{i.ToString("D5")}_streaming.wav");
            }

            //File.WriteAllText("out.txt", str);
        }
    }
}
