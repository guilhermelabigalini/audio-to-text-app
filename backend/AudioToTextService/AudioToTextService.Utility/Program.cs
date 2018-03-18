using AudioToTextService.Core.AudioConverter;
using AudioToTextService.Core.AudioDecoder;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AudioToTextService.Utility
{
    class Program
    {
        static IConfigurationRoot config;

        /*
         * converttowav "D:\temp\audio2" "D:\temp\g1.wav"
         * 
         * decodetotext "D:\temp\g1.wav" 1 pt-BR
         * decodetotext "D:\temp\audio2" 1 pt-BR
         * 
         */
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                return PrintHelpAndTerminate();
            }

            var action = args[0];

            LoadSettings();

            if (action == "converttowav" && args.Length == 3)
            {
                if (args.Length != 3)
                {
                    return PrintHelpAndTerminate();
                }
                var input = args[1];
                var output = args[2];
                return new ConverttowavHandler().Handle(config, input, output).Result;
            }
            else if (action == "decodetotext" && args.Length == 4) 
            {
                var input = args[1];
                var mode = (args[2] == "1" ? PhraseMode.ShortPhrase: PhraseMode.LongDictation);
                var locale = args[3];

                return new decodetotextHandle().Handle(config, input, mode, locale).Result;
            }

            return PrintHelpAndTerminate();
        }

        private static int PrintHelpAndTerminate()
        {
            Console.WriteLine("Invalid parameters, expected:");
            Console.WriteLine("AudioToTextService.Utility <action> [parameters...]");
            Console.WriteLine("\taction: converttowav or decodetotext");
            Console.WriteLine();
            Console.WriteLine("\t\twhen action: converttowav");
            Console.WriteLine("\t\tAudioToTextService.Utility converttowav <inputfile> <outputfile>");
            Console.WriteLine("\t\tinputfile: input audio file to be processed");
            Console.WriteLine("\t\toutputfile: output audio file, just applied when action is converttowav");
            Console.WriteLine();
            Console.WriteLine("\t\twhen action: decodetotext");
            Console.WriteLine("\t\tAudioToTextService.Utility decodetotext <inputfile> <mode> <locale>");
            Console.WriteLine("\t\tinputfile: input audio file to be processed");
            Console.WriteLine("\t\tmode: 1 for ShortPhrase, 2 for LongDictation");
            Console.WriteLine("\t\tlocale: locale of the source audio (pt-BR, en-US, ...)");
            return 1;
        }

        private static void LoadSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            config = builder.Build();
        }
    }
}
