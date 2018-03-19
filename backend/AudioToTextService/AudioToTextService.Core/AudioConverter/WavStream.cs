using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioToTextService.Core.AudioConverter
{
    public class WavStream : TempFileStream
    {
        public WavStream(string path, FileMode mode, FileAccess access, TimeSpan audioLength) : base(path, mode, access)
        {
            this.AudioLength = audioLength;
        }

        public TimeSpan AudioLength { get; private set; }
    }
}
