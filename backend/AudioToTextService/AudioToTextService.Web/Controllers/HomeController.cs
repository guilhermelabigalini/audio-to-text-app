using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AudioToTextService.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var a = System.Reflection.Assembly.GetExecutingAssembly();

            ViewBag.Version = a.GetName().Version;
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Partial()
        {
            for (int i = 1; i < 10; i++)
            {

                Response.Write("<br>" + i);
                Response.Flush();

                Thread.Sleep(2000);
            }

            return null;
        }

    }
}
