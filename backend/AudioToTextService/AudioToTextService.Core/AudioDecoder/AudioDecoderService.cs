using Microsoft.Bing.Speech;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
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
        /// A completed task
        /// </summary>
        private static readonly Task CompletedTask = Task.FromResult(true);

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
            string locale, PhraseMode mode, IAudioDecoderListener decoderListener)
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
                    Console.WriteLine("--- Partial result received by OnPartialResult ---");

                    // Print the partial response recognition hypothesis.
                    Console.WriteLine(args.DisplayText);

                    Console.WriteLine();

                    return CompletedTask;
                });

                speechClient.SubscribeToRecognitionResult((args) =>
                {
                    Console.WriteLine();

                    Console.WriteLine("--- Phrase result received by OnRecognitionResult ---");

                    // Print the recognition status.
                    Console.WriteLine("***** Phrase Recognition Status = [{0}] ***", args.RecognitionStatus);
                    if (args.Phrases != null)
                    {
                        foreach (var result in args.Phrases)
                        {
                            // Print the recognition phrase display text.
                            Console.WriteLine("{0} (Confidence:{1})", result.DisplayText, result.Confidence);
                        }
                    }

                    Console.WriteLine();
                    return CompletedTask;
                });

                // create an audio content and pass it a stream.
                var deviceMetadata = new DeviceMetadata(DeviceType.Near, DeviceFamily.Desktop, NetworkType.Ethernet, OsName.Windows, "1607", "Dell", "T3600");
                var applicationMetadata = new ApplicationMetadata("SampleApp", "1.0.0");
                var requestMetadata = new RequestMetadata(Guid.NewGuid(), deviceMetadata, applicationMetadata, "SampleAppService");

                await speechClient.RecognizeAsync(new SpeechInput(stream, requestMetadata), this.cts.Token).ConfigureAwait(false);
            }
        }
    }
}
