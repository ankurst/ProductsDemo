using System.Web.Http;
using System.Web.Mvc;

namespace ProductWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        //we can provide implementation of Application_BeginRequest() here to log request as soon as it arrives in WebAPI
    }
}
