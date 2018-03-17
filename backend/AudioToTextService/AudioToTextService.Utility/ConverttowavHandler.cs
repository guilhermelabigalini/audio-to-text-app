using AudioToTextService.Core.AudioConverter;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioToTextService.Utility
{
    class ConverttowavHandler
    {
        public async Task Handle(IConfigurationRoot config, string input, string output)
        {
            if (File.Exists(output))
            {
                Console.WriteLine("Replace destination? y/n: ");
                if (Console.Read() == (int)'y')
                    File.Delete(output);
                else
                    Environment.Exit(0);
            }

            using (FileStream istream = new FileStream(input, FileMode.Open, FileAccess.Read))
            {
                Console.WriteLine("Converting audio");
                using (Stream wavStream = await new WavAudioConverter(config).ConvertAsync(istream))
                {
                    Console.WriteLine("Converted audio");
                    Console.WriteLine("Saving audio");
                    using (FileStream ostream = new FileStream(output, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        await wavStream.CopyToAsync(ostream);
                    }
                    Console.WriteLine("Saved audio");
                }
            }
        }
    }
}
