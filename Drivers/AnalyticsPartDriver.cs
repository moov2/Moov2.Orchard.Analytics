using Moov2.Orchard.Analytics.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Moov2.Orchard.Analytics.Drivers
{
    public class AnalyticsPartDriver : ContentPartDriver<AnalyticsPart>
    {
        #region Dependencies
        private readonly IWorkContextAccessor _workContextAccessor;
        #endregion

        #region Constructor
        public AnalyticsPartDriver(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }
        #endregion

        #region Driver
        #region Display
        protected override DriverResult Display(AnalyticsPart part, string displayType, dynamic shapeHelper)
        {
            var contentItem = _workContextAccessor.GetContext().GetState<ContentItem>("currentContentItem");
            return ContentShape("Parts_Analytics", () => shapeHelper.Parts_Analytics(new { ContentItemId = contentItem?.Id }));
        }
        #endregion
        #endregion
    }
}