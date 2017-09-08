using Moov2.Orchard.Analytics.Core.User;
using Moov2.Orchard.Analytics.Models;
using Moov2.Orchard.Analytics.ViewModels;
using Orchard.Data;
using System;
using System.Web.Mvc;

namespace Moov2.Orchard.Analytics.Controllers
{
    public class RecordController : Controller
    {
        #region Dependencies
        private readonly IRepository<AnalyticsEntry> _repository;
        private readonly IUserProvider _userProvider;
        #endregion

        #region Constructor
        public RecordController(IRepository<AnalyticsEntry> repository, IUserProvider userProvider)
        {
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
            return new AnalyticsEntry
            {
                Url = model.Url,
                UserIdentifier = GetUserIdentifier(),
                VisitDateUtc = DateTime.UtcNow
            };
        }

        private string GetUserIdentifier()
        {
            var identifier = _userProvider.GetUserIdentifier();
            return !string.IsNullOrWhiteSpace(identifier) ? identifier : null;
        }
        #endregion
    }
}