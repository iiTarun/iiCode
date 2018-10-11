using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using static CentralisedUprd.Api.WebApiApplication;

namespace CentralisedUprd.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // ExceptionFilter add in config
            //config.Filters.Add(new LogExceptionFilterAttribute());

            // Web API configuration and services
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
           
            // Web API routes
            config.MapHttpAttributeRoutes();         
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
