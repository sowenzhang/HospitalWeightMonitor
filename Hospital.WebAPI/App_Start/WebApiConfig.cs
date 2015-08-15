using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Hospital.Web.Common.ErrorHandling;
using Hospital.Web.Common.Routing;

namespace Hospital.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();

            ConfigRouting(config);

            // remove tracing
            // config.Services.Replace(typeof(ITraceWriter), new );

            config.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            var cors = new EnableCorsAttribute(ConfigurationManager.AppSettings.Get("AllowedUrl"), "*", "*");
            config.EnableCors(cors);
        }

        private static void ConfigRouting(HttpConfiguration config)
        {
            var constraintsResolver = new DefaultInlineConstraintResolver();
            constraintsResolver.ConstraintMap.Add("apiVersionConstraint", typeof (ApiVersionConstraint));
            config.MapHttpAttributeRoutes(constraintsResolver);
            config.Services.Replace(typeof (IHttpControllerSelector), new NamespaceHttpControllerSelector(config));
        }
    }
}
