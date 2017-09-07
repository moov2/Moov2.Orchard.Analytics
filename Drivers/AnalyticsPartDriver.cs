using Moov2.Orchard.Analytics.Models;
using Orchard.ContentManagement.Drivers;

namespace Moov2.Orchard.Analytics.Drivers
{
    public class AnalyticsPartDriver : ContentPartDriver<AnalyticsPart>
    {
        #region Overrides
        protected override DriverResult Display(AnalyticsPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Analytics", () => shapeHelper.Parts_Analytics());
        }
        #endregion
    }
}