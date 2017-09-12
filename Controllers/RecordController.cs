using Moov2.Orchard.Analytics.Core.Settings;
using Moov2.Orchard.Analytics.Core.User;
using Moov2.Orchard.Analytics.Models;
using Moov2.Orchard.Analytics.ViewModels;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Tags.Models;
using System;
using System.Web.Mvc;

namespace Moov2.Orchard.Analytics.Controllers
{
    public class RecordController : Controller
    {
        #region Dependencies
        private readonly IAnalyticsSettings _analyticsSettings;
        private readonly IContentManager _contentManager;
        private readonly IRepository<AnalyticsEntry> _repository;
        private readonly IUserProvider _userProvider;
        #endregion

        #region Constructor
        public RecordController(IAnalyticsSettings analyticsSettings, IContentManager contentManager, IRepository<AnalyticsEntry> repository, IUserProvider userProvider)
        {
            _analyticsSettings = analyticsSettings;
            _contentManager = contentManager;
            _repository = repository;
            _userProvider = userProvider;
        }
        #endregion

        #region Actions
        [HttpPost]
        public ActionResult Index(AnalyticsEntryViewModel model)
        {
            _repository.Create(ConvertToEntry(model));

            return new HttpStatusCodeResult(200);
        }
        #endregion

        #region Helpers
        private AnalyticsEntry ConvertToEntry(AnalyticsEntryViewModel model)
        {
            var entry = new AnalyticsEntry
            {
                Url = model.Url,
                UserIdentifier = GetUserIdentifier(),
                VisitDateUtc = DateTime.UtcNow
            };
            var contentItem = model.ContentItemId.HasValue ? _contentManager.Get(model.ContentItemId.Value, VersionOptions.Published) : null;
            if (contentItem == null)
                return entry;
            entry.ContentItemId = contentItem.Id;
            var tagsPart = contentItem.As<TagsPart>();
            if (tagsPart == null)
                return entry;
            entry.Tags = string.Join(",", tagsPart.CurrentTags);
            return entry;
        }

        private string GetUserIdentifier()
        {
            var identifier = _userProvider.GetUserIdentifier();
            return !string.IsNullOrWhiteSpace(identifier) ? identifier : null;
        }
        #endregion
    }
}