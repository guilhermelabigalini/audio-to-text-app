using AudioToTextService.Core.AudioConverter;
using AudioToTextService.Core.AudioDecoder;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AudioToTextService.Web.Controllers
{
    public class DecodeController : Controller
    {
        private static readonly Task CompletedTask = Task.FromResult(true);
        private static TimeSpan MaxShortAudioLength = TimeSpan.FromMinutes(14);

        private IConfiguration configuration;

        public DecodeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // GET: Decode
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Post(string culture = "pt-BR")
        {
            /*
            foreach (String key in Request.Files)
            {
                HttpPostedFileBase f = Request.Files[key];
                Response.Write($"filename {f.FileName} size {f.ContentLength}");
            }
            */

            if (Request.Files.Count != 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var file = Request.Files[0];

            Response.Buffer = false;
            Response.ContentType = "text/plain";

            using (WavStream wavStream = await new WavAudioConverter(configuration).ConvertAsync(file.InputStream))
            {
                /*
                 * ShortPhrase mode: An utterance up to 15 seconds long. As data is sent to the server, 
                 * the client receives multiple partial results and one final best result.
                 * 
                 * LongDictation mode: An utterance up to 10 minutes long. As data is sent to the server, 
                 * the client receives multiple partial results and multiple final results, based on where the 
                 * server indicates sentence pauses.
                */
                PhraseMode mode = (wavStream.AudioLength > MaxShortAudioLength ?
                        PhraseMode.LongDictation :
                        PhraseMode.ShortPhrase);

                await new AudioDecoderService(configuration).DecodeAudioAsync(wavStream, culture, mode, 
                    (args) =>
                    {
                        Response.Write(JsonConvert.SerializeObject(args));
                        Response.Write(Environment.NewLine);
                        return Response.FlushAsync();
                    },
                    (args) =>
                    {
                        Response.Write(JsonConvert.SerializeObject(args));
                        Response.Write(Environment.NewLine);
                        return Response.FlushAsync();
                    });
            }

            return null;
        }
    }
}