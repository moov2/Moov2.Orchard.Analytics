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
                },
                new RouteDescriptor {
                    Name = Constants.ADMIN_ROUTE_NAME,
                    Route = new Route(
                        "Admin/Analytics",
                        new RouteValueDictionary {
                            {"area", "Moov2.Orchard.Analytics"},
                            {"controller", "Admin"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Moov2.Orchard.Analytics"}
                         },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Name = Constants.ADMIN_BYPAGE_ROUTE_NAME,
                    Route = new Route(
                        "Admin/Analytics/ByPage",
                        new RouteValueDictionary {
                            {"area", "Moov2.Orchard.Analytics"},
                            {"controller", "Admin"},
                            {"action", "ByPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Moov2.Orchard.Analytics"}
                         },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Name = Constants.ADMIN_BYUSER_ROUTE_NAME,
                    Route = new Route(
                        "Admin/Analytics/ByUser",
                        new RouteValueDictionary {
                            {"area", "Moov2.Orchard.Analytics"},
                            {"controller", "Admin"},
                            {"action", "ByUser"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Moov2.Orchard.Analytics"}
                         },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Name = Constants.ADMIN_BYTAG_ROUTE_NAME,
                    Route = new Route(
                        "Admin/Analytics/ByTag",
                        new RouteValueDictionary {
                            {"area", "Moov2.Orchard.Analytics"},
                            {"controller", "Admin"},
                            {"action", "ByTag"}
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