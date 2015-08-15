using System.Web.Http;

namespace Hospital.Web.Common.Routing
{
    /// <summary>
    /// it indicates the Api version prefix as api/v1/[whatever] 
    /// </summary>
    /// <remarks>
    /// if we want a different 
    /// </remarks>
    public class ApiVersionRoutePrefixAttribute : RoutePrefixAttribute
    {
        private const string RouteBase = "api/{apiVersion:apiVersionConstraint(v1)}";
        // private const string PrefixRouteBase = RouteBase + "/";

        public ApiVersionRoutePrefixAttribute(string routePrefix, int version = 1)
            : base(GetVersionedRoute(routePrefix, version))
        {
        }

        private static string GetVersionedRoute(string routePrefix, int version)
        {
            string route = string.IsNullOrWhiteSpace(routePrefix) ? "" : "/" + routePrefix;

            if (version == 1)
            {
                return RouteBase + route;
            }

            if (version == 2)
            {
                return RouteBase.Replace("v1", "v2") + route;
            }

            return routePrefix;
        }
    }
}