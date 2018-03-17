using AudioToTextService.Core.AudioConverter;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AudioToTextService.Utility
{
    class Program
    {
        static IConfigurationRoot config;

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid parameters, expected:");
                Console.WriteLine("AudioToTextService.Utility <action> <inputfile> [outputfile]");
                Console.WriteLine("\taction: converttowav or decodetotext");
                Console.WriteLine("\tinputfile: input audio file to be processed");
                Console.WriteLine("\toutputfile: output audio file, just applied when action is converttowav");
            }

            var action = args[0];
            var input = args[1];
            var output = (args.Length >= 3 ? args[2] : null);

            LoadSettings();

            if (action == "converttowav")
                new ConverttowavHandler().Handle(config, input, output).Wait();

            if (action == "decode")
                Handle_decode(config, input);

        }

        private static void LoadSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            config = builder.Build();
        }

        private static void Handle_decode(IConfigurationRoot config, string input)
        {
            throw new NotImplementedException();
        }

        
    }
}
