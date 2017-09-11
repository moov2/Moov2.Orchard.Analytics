using Moov2.Orchard.Analytics.Core.Queries;
using Moov2.Orchard.Analytics.ViewModels.Admin;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Settings;
using Orchard.Themes;
using Orchard.UI.Navigation;
using System.Web.Mvc;

namespace Moov2.Orchard.Analytics.Controllers
{
    [Themed]
    public class AdminController : Controller
    {
        #region Dependencies
        private readonly IAnalyticsQueries _analyticsQueries;
        private readonly IAuthorizer _authorizer;
        private readonly dynamic _shape;
        private readonly ISiteService _siteService;

        public Localizer T { get; set; }
        #endregion

        #region Constructor
        public AdminController(IAnalyticsQueries analyticsQueries, IAuthorizer authorizer, IShapeFactory shapeFactory, ISiteService siteService)
        {
            T = NullLocalizer.Instance;

            _analyticsQueries = analyticsQueries;
            _authorizer = authorizer;
            _shape = shapeFactory;
            _siteService = siteService;
        }
        #endregion

        #region Actions
        public ActionResult Index(PagerParameters pagerParameters)
        {
            if (!_authorizer.Authorize(Permissions.ViewAnalytics, T("You are not allowed to view analytics, missing View Analytics permission.")))
                return new HttpUnauthorizedResult();

            var total = _analyticsQueries.GetAllCount();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            var pagerShape = _shape.Pager(pager).TotalItemCount(total);

            var entries = _analyticsQueries.GetAll(pager.GetStartIndex(), pager.PageSize);

            return View(new RawAnalyticsViewModel { Entries = entries, Pager = pagerShape });
        }

        public ActionResult ByPage(PagerParameters pagerParameters)
        {
            if (!_authorizer.Authorize(Permissions.ViewAnalytics, T("You are not allowed to view analytics, missing View Analytics permission.")))
                return new HttpUnauthorizedResult();

            var total = _analyticsQueries.GetByPageCount();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            var pagerShape = _shape.Pager(pager).TotalItemCount(total);

            var entries = _analyticsQueries.GetByPage(pager.GetStartIndex(), pager.PageSize);

            return View(new ByPageAnalyticsViewModel { Entries = entries, Pager = pagerShape });
        }

        public ActionResult ByUser(PagerParameters pagerParameters)
        {
            if (!_authorizer.Authorize(Permissions.ViewAnalytics, T("You are not allowed to view analytics, missing View Analytics permission.")))
                return new HttpUnauthorizedResult();

            var total = _analyticsQueries.GetByUserCount();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            var pagerShape = _shape.Pager(pager).TotalItemCount(total);

            var entries = _analyticsQueries.GetByUser(pager.GetStartIndex(), pager.PageSize);

            return View(new ByUserAnalyticsViewModel { Entries = entries, Pager = pagerShape });
        }
        #endregion
    }
}