using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.DisplayManagement;

namespace Moov2.Orchard.Analytics.Handlers
{
    public class CurrentContentItemHandler : ContentHandler
    {
        #region Dependencies
        private readonly IOrchardServices _orchardServices;
        #endregion

        #region Constructor
        public CurrentContentItemHandler(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }
        #endregion

        #region ContentHandler
        protected override void BuildDisplayShape(BuildDisplayContext context)
        {
            if (context.DisplayType == "Detail" &&
                ((IShape)context.Shape).Metadata.Type == "Content" &&
                _orchardServices.WorkContext.GetState<ContentItem>("currentContentItem") == null)
            {
                _orchardServices.WorkContext.SetState("currentContentItem", context.ContentItem);
            }
        }
        #endregion
    }
}