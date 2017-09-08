using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moov2.Orchard.Analytics
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                new RouteDescriptor {
                    Name = Constants.RECORD_ROUTE_NAME,
                    Route = new Route(
                        "views/record",
                        new RouteValueDictionary {
                            {"area", "Moov2.Orchard.Analytics"},
                            {"controller", "Record"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Moov2.Orchard.Analytics"}
                         },
                        new MvcRouteHandler())
                }
            };
        }
    }
}