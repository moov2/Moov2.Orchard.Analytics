using CsvHelper;
using Moov2.Orchard.Analytics.Core.Queries;
using Moov2.Orchard.Analytics.Core.Settings;
using Moov2.Orchard.Analytics.ViewModels.Admin;
using Moov2.Orchard.Analytics.ViewModels.Admin.Dto;
using Orchard.Core.Common.ViewModels;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Settings;
using Orchard.Themes;
using Orchard.UI.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace Moov2.Orchard.Analytics.Controllers
{
    [Themed]
    public class AdminController : Controller
    {
        #region Dependencies

        private readonly IAnalyticsQueries _analyticsQueries;
        private readonly IAnalyticsSettings _analyticsSettings;
        private readonly IAuthorizer _authorizer;
        private readonly dynamic _shape;
        private readonly ISiteService _siteService;

        public Localizer T { get; set; }

        #endregion

        #region Constructor

        public AdminController(IAnalyticsQueries analyticsQueries, IAnalyticsSettings analyticsSettings, IAuthorizer authorizer, IShapeFactory shapeFactory, ISiteService siteService)
        {
            T = NullLocalizer.Instance;

            _analyticsQueries = analyticsQueries;
            _analyticsSettings = analyticsSettings;
            _authorizer = authorizer;
            _shape = shapeFactory;
            _siteService = siteService;
        }

        #endregion

        #region Actions

        public ActionResult Index(PagerParameters pagerParameters, AnalyticsViewModel<RawAnalyticsDto> model)
        {
            return GetAnalyticsResults(pagerParameters, model, query => _analyticsQueries.GetAllCount(query), query => _analyticsQueries.GetAll(query));
        }

        public ActionResult ByPage(PagerParameters pagerParameters, AnalyticsViewModel<SingleStatDto> model)
        {
            return GetAnalyticsResults(pagerParameters, model, query => _analyticsQueries.GetByPageCount(query), query => _analyticsQueries.GetByPage(query));
        }

        public ActionResult ByUser(PagerParameters pagerParameters, AnalyticsViewModel<SingleStatDto> model)
        {
            return GetAnalyticsResults(pagerParameters, model, query => _analyticsQueries.GetByUserCount(query), query => _analyticsQueries.GetByUser(query));
        }

        public ActionResult ByTag(PagerParameters pagerParameters, AnalyticsViewModel<SingleStatDto> model)
        {
            if (!_analyticsSettings.TagsEnabled)
                return HttpNotFound();

            return GetAnalyticsResults(pagerParameters, model, x => _analyticsQueries.GetByTagCount(x), x => _analyticsQueries.GetByTag(x));
        }

        #endregion

        #region Helpers

        private ActionResult GetAnalyticsResults<D>(PagerParameters pagerParameters, AnalyticsViewModel<D> model, Func<AnalyticsQueryModel, int> count, Func<AnalyticsQueryModel, IList<D>> entries)
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
            model.TagsEnabled = _analyticsSettings.TagsEnabled;

            if (!model.DownloadCsv)
                return View(model);

            return DownloadCsv(model);
        }

        private AnalyticsQueryModel QueryModelForViewModel(Pager pager, AnalyticsViewModel model)
        {
            var query = new AnalyticsQueryModel
            {
                Skip = model.DownloadCsv ? 0 : pager.GetStartIndex(),
                Take = model.DownloadCsv ? 0 : pager.PageSize
            };

            DateTime parsed;

            if (!string.IsNullOrWhiteSpace(model.From.Date) && DateTime.TryParse(model.From.Date, out parsed))
                query.FromUtc = parsed.ToUniversalTime();

            if (!string.IsNullOrWhiteSpace(model.To.Date) && DateTime.TryParse(model.To.Date, out parsed))
                query.ToUtc = parsed.ToUniversalTime();

            query.Term = model.Term;
            return query;
        }

        private ActionResult DownloadCsv<D>(AnalyticsViewModel<D> model)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            var csvWriter = new CsvWriter(sw);

            csvWriter.WriteHeader<D>();

            foreach (var entry in model.Entries)
            {
                csvWriter.WriteRecord(entry);
            }

            sw.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            return File(ms, "text/csv", $"analytics-{DateTime.Now:yyyy-MM-dd-HH-mm}.csv");
        }

        #endregion
    }
}