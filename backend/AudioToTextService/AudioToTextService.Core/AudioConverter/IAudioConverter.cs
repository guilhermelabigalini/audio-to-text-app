using System.IO;
using System.Threading.Tasks;

namespace AudioToTextService.Core
{
    public interface IAudioConverter
    {
        Task<Stream> ConvertAsync(Stream stream);
    }
}
