using Orchard;

namespace Moov2.Orchard.Analytics.Core.Settings
{
    public interface IAnalyticsSettings : IDependency
    {
        bool TagsEnabled { get; }
    }
}
