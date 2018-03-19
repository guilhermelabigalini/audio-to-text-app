using Microsoft.Bing.Speech;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AudioToTextService.Core.AudioDecoder
{
    public enum PhraseMode
    {
        ShortPhrase,

        LongDictation
    }

    public class AudioDecoderService
    {
        /// <summary>
        /// Short phrase mode URL
        /// </summary>
        private static readonly Uri ShortPhraseUrl = new Uri(@"wss://speech.platform.bing.com/api/service/recognition");

        /// <summary>
        /// The long dictation URL
        /// </summary>
        private static readonly Uri LongDictationUrl = new Uri(@"wss://speech.platform.bing.com/api/service/recognition/continuous");

        /// <summary>
        /// Cancellation token used to stop sending the audio.
        /// </summary>
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        private IConfiguration configuration;

        public AudioDecoderService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task DecodeAudioAsync(
            Stream stream, 
            string locale, PhraseMode mode, 
            Func<RecognitionStep, Task> partialResult,
            Func<RecognitionFinalResult, Task> finalResult)
        {
            var serviceUrl = (mode == PhraseMode.LongDictation ? LongDictationUrl : ShortPhraseUrl);

            string subscriptionKey = configuration.GetSection("AudioToTextService.Core.AudioDecoder")["subscriptionKey"];

            // create the preferences object
            var preferences = new Preferences(locale, serviceUrl, new CognitiveServicesAuthorizationProvider(subscriptionKey));

            // Create a a speech client
            using (var speechClient = new SpeechClient(preferences))
            {
                speechClient.SubscribeToPartialResult((args) =>
                {
                    return partialResult(ToStep(args));
                });

                speechClient.SubscribeToRecognitionResult((args) =>
                {
                    return finalResult(ToFinalResult(args));
                });

                // create an audio content and pass it a stream.
                var deviceMetadata = new DeviceMetadata(DeviceType.Near, DeviceFamily.Desktop, NetworkType.Ethernet, OsName.Windows, "1607", "Dell", "T3600");
                var applicationMetadata = new ApplicationMetadata("SampleApp", "1.0.0");
                var requestMetadata = new RequestMetadata(Guid.NewGuid(), deviceMetadata, applicationMetadata, "SampleAppService");

                await speechClient.RecognizeAsync(new SpeechInput(stream, requestMetadata), this.cts.Token).ConfigureAwait(false);
            }
        }

        private RecognitionFinalResult ToFinalResult(RecognitionResult args)
        {
            return new RecognitionFinalResult()
            {
                RecognitionStatus = (RecognitionFinalStatus)(int)args.RecognitionStatus,
                Phrases = args.Phrases?.Select(i => ToStep(i)).ToList()                
            };
        }

        private RecognitionStep ToStep(RecognitionPhrase args)
        {
            return new RecognitionStep()
            {
                Confidence = (RecognitionConfidence)(int)args.Confidence,
                DisplayText = args.DisplayText,
                LexicalForm = args.LexicalForm,
                MediaDuration = args.MediaDuration,
                MediaTime = args.MediaTime
            };
        }
    }
}
