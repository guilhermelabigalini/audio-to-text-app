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
        private static readonly Task CompletedTask = Task.FromResult(true);

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
                    await new AudioDecoderService(config).DecodeAudioAsync(wavStream, locale, mode, 
                        (args) =>
                        {
                            Console.WriteLine("--- Partial result received by OnPartialResult ---");

                            // Print the partial response recognition hypothesis.
                            Console.WriteLine(args.DisplayText);

                            Console.WriteLine();

                            return CompletedTask;
                        },
                        (args) =>
                        {
                            Console.WriteLine();

                            Console.WriteLine("--- Phrase result received by OnRecognitionResult ---");

                            // Print the recognition status.
                            Console.WriteLine("***** Phrase Recognition Status = [{0}] ***", args.RecognitionStatus);
                            if (args.Phrases != null)
                            {
                                foreach (var result in args.Phrases)
                                {
                                    // Print the recognition phrase display text.
                                    Console.WriteLine("{0} (Confidence:{1})", result.DisplayText, result.Confidence);
                                }
                            }

                            Console.WriteLine();
                            return CompletedTask;
                        });
                    Console.WriteLine("Audio decoded");
                }
            }

            return 0;
        }
    }
}
