using Orchard.Environment.Descriptor.Models;
using System;
using System.Linq;

namespace Moov2.Orchard.Analytics.Core.Extensions
{
    public static class ShellDescriptorExtensions
    {
        public static bool IsAnalyticsTagsEnabled(this ShellDescriptor shellDescriptor)
        {
            return shellDescriptor?.Features.Any(x => Features.AnalyticsTags.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase)) ?? false;
        }
    }
}