using Moov2.Orchard.Analytics.Core.Queries;
using Moov2.Orchard.Analytics.ViewModels.Admin;
using Orchard.Core.Common.ViewModels;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Settings;
using Orchard.Themes;
using Orchard.UI.Navigation;
using System;
using System.Collections.Generic;
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
        public ActionResult Index(PagerParameters pagerParameters, RawAnalyticsViewModel model)
        {
            return View(GetAnalyticsResults(pagerParameters, model, query => _analyticsQueries.GetAllCount(query), query => _analyticsQueries.GetAll(query)));
        }

        public ActionResult ByPage(PagerParameters pagerParameters, ByPageAnalyticsViewModel model)
        {
            return View(GetAnalyticsResults(pagerParameters, model, query => _analyticsQueries.GetByPageCount(query), query => _analyticsQueries.GetByPage(query)));
        }

        public ActionResult ByUser(PagerParameters pagerParameters, ByUserAnalyticsViewModel model)
        {
            return View(GetAnalyticsResults(pagerParameters, model, query => _analyticsQueries.GetByUserCount(query), query => _analyticsQueries.GetByUser(query)));
        }
        #endregion

        #region Helpers
        private AnalyticsViewModel<D> GetAnalyticsResults<D>(PagerParameters pagerParameters, AnalyticsViewModel<D> model, Func<AnalyticsQueryModel, int> count, Func<AnalyticsQueryModel, IList<D>> entries)
        {
            if (!_authorizer.Authorize(Permissions.ViewAnalytics, T("You are not allowed to view analytics, missing View Analytics permission.")))
                throw new UnauthorizedAccessException();

            if (model.From == null)
                model.From = new DateTimeEditor();
            model.From.ShowDate = true;

            if (model.To == null)
                model.To = new DateTimeEditor();
            model.To.ShowDate = true;

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            var queryModel = QueryModelForViewModel(pager, model);

            var total = count(queryModel);

            var pagerShape = _shape.Pager(pager).TotalItemCount(total);

            model.Entries = entries(queryModel);
            model.Pager = pagerShape;
            return model;
        }

        private AnalyticsQueryModel QueryModelForViewModel(Pager pager, AnalyticsViewModel model)
        {
            var query = new AnalyticsQueryModel
            {
                Skip = pager.GetStartIndex(),
                Take = pager.PageSize
            };
            DateTime parsed;
            if (!string.IsNullOrWhiteSpace(model.From.Date) && DateTime.TryParse(model.From.Date, out parsed))
                query.FromUtc = parsed.ToUniversalTime();
            if (!string.IsNullOrWhiteSpace(model.To.Date) && DateTime.TryParse(model.To.Date, out parsed))
                query.ToUtc = parsed.ToUniversalTime();
            return query;
        }
        #endregion
    }
}