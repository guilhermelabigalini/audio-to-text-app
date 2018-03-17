using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AudioToTextService.Core
{
    internal class TempFileInfo : IDisposable
    {
        public TempFileInfo(string fileName)
        {
            FileName = fileName;
        }

        public String FileName { get; }

        public void Dispose()
        {
            File.Delete(this.FileName);
        }

        public static async Task<TempFileInfo> GenTempFileAsync(Stream stream)
        {
            string path = Path.GetTempFileName();
            using (FileStream tempFileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                await stream.CopyToAsync(tempFileStream);
            }

            return new TempFileInfo(path);
        }
    }
}
