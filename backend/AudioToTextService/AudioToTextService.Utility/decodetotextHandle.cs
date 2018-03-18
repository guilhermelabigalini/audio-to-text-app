using AudioToTextService.Core.AudioConverter;
using AudioToTextService.Core.AudioDecoder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioToTextService.Utility
{
    class decodetotextHandle
    {
        public async Task<int> Handle(IConfigurationRoot config, string inputFile, PhraseMode mode, string locale)
        {
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Invalid input file");
                return 1;
            }

            using (FileStream istream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            {
                Console.WriteLine("Converting audio");
                using (Stream wavStream = await new WavAudioConverter(config).ConvertAsync(istream))
                {
                    Console.WriteLine("Converted audio");
                    Console.WriteLine("Decoding audio");
                    await new AudioDecoderService(config).DecodeAudioAsync(wavStream, locale, mode, null);
                    Console.WriteLine("Audio decoded");
                }
            }

            return 0;
        }
    }
}
