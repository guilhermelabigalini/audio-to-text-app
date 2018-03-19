using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioToTextService.Core.AudioDecoder
{
    public enum RecognitionConfidence
    {
        None = 0,
        High = 1,
        Normal = 2,
        Low = 3
    }

    public class RecognitionStep
    {
        public string DisplayText { get; set; }

        public string LexicalForm { get; set; }

        public RecognitionConfidence Confidence { get; set; }

        public ulong MediaTime { get; set; }

        public uint MediaDuration { get; set; }
    }


}
