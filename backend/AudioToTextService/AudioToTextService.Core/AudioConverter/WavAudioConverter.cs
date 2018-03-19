using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AudioToTextService.Core.AudioConverter
{
    public class WavAudioConverter : IAudioConverter
    {
        private const string ToWavCmdLine = "-i \"{0}\" -acodec pcm_u8 -ac 1 -ar 16000 \"{1}\"";
        private IConfiguration configuration;

        public WavAudioConverter(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<WavStream> ConvertAsync(Stream stream)
        {
            var section = configuration.GetSection("AudioToTextService.Core.WavAudioConverter");
            var ffmpegPath = section["ffmpeg"];

            using (var inputFile = await TempFileInfo.GenTempFileAsync(stream))
            {
                var outputFile = Path.ChangeExtension(Path.GetTempFileName(), ".wav");
                var cmd = String.Format(ToWavCmdLine, inputFile.FileName, outputFile);

                ProcessStartInfo psi = new ProcessStartInfo(ffmpegPath, cmd)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process p = Process.Start(psi))
                {
                    //* Read the output (or the error)
                    string ffmpegOutput = p.StandardOutput.ReadToEnd();
                    string ffmpegErrOutput = p.StandardError.ReadToEnd();

                    p.WaitForExit();

                    if (p.ExitCode != 0)
                    {
                        throw new Exception(ffmpegOutput + ffmpegErrOutput);
                    }

                    TimeSpan ts = ParseTimeFromFFMpegOutput(ffmpegErrOutput);

                    return new WavStream(outputFile, FileMode.Open, FileAccess.Read, ts);
                }
            }
        }

        private static TimeSpan ParseTimeFromFFMpegOutput(string ffmpegErrOutput)
        {
            TimeSpan ts = TimeSpan.Zero;
            try
            {
                const string TimeKey = "time=";
                int timePos = ffmpegErrOutput.IndexOf(TimeKey);
                if (timePos > 0)
                {
                    int timeEndPos = ffmpegErrOutput.IndexOf(" ", timePos + 1);
                    if (timeEndPos > 0)
                    {
                        string timeStr = ffmpegErrOutput.Substring(timePos + TimeKey.Length, timeEndPos - timePos - TimeKey.Length);
                        ts = TimeSpan.Parse(timeStr);
                    }
                }
            }
            catch (Exception)
            { }

            return ts;
        }
    }
}
