using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioToTextService.Core.AudioDecoder
{
    public enum RecognitionFinalStatus
    {
        None = 0,
        Success = 200,
        NoMatch = 201,
        InitialSilenceTimeout = 202,
        PhraseSilenceTimeout = 203,
        BabbleTimeout = 204,
        Cancelled = 205
    }

    public class RecognitionFinalResult
    {
        public RecognitionFinalStatus RecognitionStatus { get; set; }
        public List<RecognitionStep> Phrases { get; set;  }
    }
}
