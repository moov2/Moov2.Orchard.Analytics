using Moov2.Orchard.Analytics.Core.Extensions;
using Orchard.Environment.Descriptor.Models;

namespace Moov2.Orchard.Analytics.Core.Settings
{
    public class AnalyticsSettings : IAnalyticsSettings
    {
        #region Dependencies
        private readonly ShellDescriptor _shellDescriptor;
        #endregion

        #region Constructor
        public AnalyticsSettings(ShellDescriptor shellDescriptor)
        {
            _shellDescriptor = shellDescriptor;
        }
        #endregion

        #region IAnalyticsSettings
        public bool TagsEnabled => _shellDescriptor.IsAnalyticsTagsEnabled();
        #endregion
    }
}