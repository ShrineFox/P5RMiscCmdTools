using System;
using NAudio.Wave;
using SoundTouch;
using SoundTouch.Net;

namespace ShadowVoiceEffect
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(var wavFile in Directory.GetFiles(args[0], "*.wav", SearchOption.TopDirectoryOnly))
            {
                string inputPath = wavFile;
                string outputPath = Path.Combine(Path.GetDirectoryName(wavFile), "shadow" + Path.GetFileName(wavFile));

                using (var reader = new AudioFileReader(inputPath))
                {
                    int sampleRate = reader.WaveFormat.SampleRate;
                    int channels = reader.WaveFormat.Channels;

                    // Read all samples (AudioFileReader returns floats)
                    int totalFloats = (int)(reader.Length / sizeof(float));
                    var samples = new float[totalFloats];
                    int floatsRead = reader.Read(samples, 0, totalFloats);

                    // Pitch up (+24 semitones = +2 octaves)
                    var pitchedUp = PitchShift(samples, floatsRead, sampleRate, channels, 3f);

                    // Pitch down (-12 semitones = -1 octave)
                    var pitchedDown = PitchShift(samples, floatsRead, sampleRate, channels, -3f);

                    // Mix original + up + down with volume weights
                    float upVolume = 0.4f;   // 30% volume for pitched up
                    float downVolume = 0.4f; // 30% volume for pitched down
                    float origVolume = 1.0f; // full volume for original

                    // Mix original + up + down
                    int length = Math.Min(floatsRead, Math.Min(pitchedUp.Length, pitchedDown.Length));
                    var mixed = new float[length];
                    for (int i = 0; i < length; i++)
                    {
                        float orig = samples[i] * origVolume;
                        float up = pitchedUp[i] * upVolume;
                        float down = pitchedDown[i] * downVolume;

                        mixed[i] = orig + up + down;

                        // optional: prevent clipping
                        //if (mixed[i] > 1f) mixed[i] = 1f;
                        //if (mixed[i] < -1f) mixed[i] = -1f;
                    }

                    // Write result as IEEE float WAV (widely supported)
                    var outFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
                    using (var writer = new WaveFileWriter(outputPath, outFormat))
                    {
                        writer.WriteSamples(mixed, 0, mixed.Length);
                    }
                }
            }
        }

        // PitchShift: uses SoundTouch.Net's SoundTouch class.
        // input: float[] (interleaved samples), floatCount = number of floats in input to process
        // semitones: positive to raise pitch, negative to lower
        static float[] PitchShift(float[] input, int floatCount, int sampleRate, int channels, float semitones)
        {
            // Create SoundTouch instance (SoundTouch.Net)
            var st = new SoundTouchProcessor(); // fully-qualified to avoid ambiguity
            st.SampleRate = sampleRate;
            st.Channels = channels;
            st.PitchSemiTones = semitones; 

            // Put samples (SoundTouch expects number of frames: floats / channels)
            int frames = floatCount / channels;
            st.PutSamples(input, frames);

            // Tell SoundTouch we are done feeding this stream
            st.Flush();

            // Read out processed frames in a loop (append them)
            var outList = new List<float>();
            var temp = new float[4096 * channels]; // temp buffer (frames*channels)
            int receivedFrames;
            do
            {
                // ReceiveSamples expects max frames to read; returns number of frames received
                receivedFrames = st.ReceiveSamples(temp, temp.Length / channels);
                if (receivedFrames > 0)
                {
                    int floats = receivedFrames * channels;
                    for (int i = 0; i < floats; i++)
                        outList.Add(temp[i]);
                }
            } while (receivedFrames != 0);

            return outList.ToArray();
        }
    }


}
