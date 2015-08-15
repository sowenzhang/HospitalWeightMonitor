using System;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Hospital.WebAPI;
using Microsoft.Owin;
using Microsoft.Owin.BuilderProperties;
using Microsoft.Owin.Cors;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Hospital.WebAPI
{
    public class Startup: IDisposable
    {
        private IDependencyResolver _resolver;

        public void Configuration(IAppBuilder app)
        {
            string executable = HttpContext.Current.Server.MapPath("~/");
            string path = (System.IO.Path.GetDirectoryName(executable)) + "\\..\\Database\\";
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            app.UseCors(CorsOptions.AllowAll);
            ConfigResolver();
            ConfigureWebApiStartup(app);
            ConfigureShutdown(app);
        }

        private void ConfigResolver()
        {
            // Use UnityHierarchicalDependencyResolver if you want to use a new child container for each IHttpController resolution.
            // var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.GetConfiguredContainer());
            _resolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());

            // I have to set this, because WebContainerManager uses this GlobalConfiguration to find the resolver 
            GlobalConfiguration.Configuration.DependencyResolver = _resolver;
        }

        private void ConfigureWebApiStartup(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.DependencyResolver = _resolver;
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

        private void ConfigureShutdown(IAppBuilder app)
        {
            var properties = new AppProperties(app.Properties);
            CancellationToken token = properties.OnAppDisposing;
            if (token != CancellationToken.None)
            {
                token.Register(() =>
                {
                    IUnityContainer container = UnityConfig.GetConfiguredContainer();
                    container.Dispose();
                    Dispose();
                });
            }
        }

        private bool _isDisposing;
        public void Dispose()
        {
            if (_isDisposing) return;

            _isDisposing = true;
            if (_resolver != null)
                _resolver.Dispose();
        }
    }
}