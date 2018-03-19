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
        public async System.Threading.Tasks.Task<ActionResult> PostAsync(string culture)
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

            using (Stream wavStream = await new WavAudioConverter(configuration).ConvertAsync(file.InputStream))
            {
                PhraseMode mode = PhraseMode.ShortPhrase;
                await new AudioDecoderService(configuration).DecodeAudioAsync(wavStream, culture, mode, 
                    (args) =>
                    {
                        Response.Write(JsonConvert.SerializeObject(args));
                        return Response.FlushAsync();
                    },
                    (args) =>
                    {
                        Response.Write(JsonConvert.SerializeObject(args));
                        return Response.FlushAsync();
                    });
            }

            return null;
        }
    }
}