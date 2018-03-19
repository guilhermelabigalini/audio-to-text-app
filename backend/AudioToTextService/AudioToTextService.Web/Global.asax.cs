using AudioToTextService.Web.App_Start;
using Microsoft.Extensions.Configuration;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace AudioToTextService.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IConfiguration LoadSettings()
        {
           var currentDir = Server.MapPath("~/App_Data");

            var builder = new ConfigurationBuilder()
                .SetBasePath(currentDir)
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            return config;
        }

        private void StartIOC()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IConfiguration>().ToConstant(LoadSettings());

            NinjectDependencyResolver ninjectResolver = new NinjectDependencyResolver(kernel);
            DependencyResolver.SetResolver(ninjectResolver);
            GlobalConfiguration.Configuration.DependencyResolver = ninjectResolver; //Web API
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            StartIOC();
        }
    }
}
