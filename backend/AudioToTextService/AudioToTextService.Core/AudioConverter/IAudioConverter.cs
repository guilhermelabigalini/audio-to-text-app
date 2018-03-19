using AudioToTextService.Core.AudioConverter;
using System.IO;
using System.Threading.Tasks;

namespace AudioToTextService.Core.AudioConverter
{
    public interface IAudioConverter
    {
        Task<WavStream> ConvertAsync(Stream stream);
    }
}
