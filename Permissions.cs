using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System.Collections.Generic;

namespace Moov2.Orchard.Analytics
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ViewAnalytics = new Permission { Description = "View page view analytics data", Name = "ViewAnalytics" };

        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { ViewAnalytics };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new [] { ViewAnalytics }
                }
            };
        }
    }
}