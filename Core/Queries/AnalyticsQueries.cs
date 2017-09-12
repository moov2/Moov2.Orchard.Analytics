using Moov2.Orchard.Analytics.Core.Settings;
using Moov2.Orchard.Analytics.Models;
using Moov2.Orchard.Analytics.ViewModels.Admin.Dto;
using NHibernate.Linq;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Tags.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Moov2.Orchard.Analytics.Core.Queries
{
    public class AnalyticsQueries : IAnalyticsQueries
    {
        #region Dependencies
        private readonly IAnalyticsSettings _analyticsSettings;
        private readonly IRepository<AnalyticsEntry> _repository;
        private readonly ITagService _tagService;
        private readonly ITransactionManager _transactionManager;

        public Localizer T { get; set; }
        #endregion

        #region Constructor
        public AnalyticsQueries(IAnalyticsSettings analyticsSettings, IRepository<AnalyticsEntry> repository, ITagService tagService, ITransactionManager transactionManager)
        {
            T = NullLocalizer.Instance;

            _analyticsSettings = analyticsSettings;
            _repository = repository;
            _tagService = tagService;
            _transactionManager = transactionManager;
        }
        #endregion

        #region IAnalyticsQueries
        public IList<RawAnalyticsDto> GetAll(AnalyticsQueryModel query)
        {
            var queryable = GetQueryable();
            queryable = ApplyFilters(queryable, query);
            queryable = queryable.OrderByDescending(x => x.VisitDateUtc)
                .Skip(query.Skip);
            if (query.Take > 0)
                queryable = queryable.Take(query.Take);
            var items = queryable
                .Select(x => new RawAnalyticsDto
                {
                    Tags = x.Tags,
                    UserIdentifier = !string.IsNullOrWhiteSpace(x.UserIdentifier) ? x.UserIdentifier : T("Anonymous").ToString(),
                    Url = x.Url,
                    VisitDate = x.VisitDateUtc.ToLocalTime()
                }).ToList();
            foreach (var item in items)
            {
                var tags = (item.Tags ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Array.Sort(tags);
                item.Tags = string.Join(", ", tags);
            }
            return items;
        }

        public int GetAllCount(AnalyticsQueryModel query)
        {
            var queryable = GetQueryable();
            queryable = ApplyFilters(queryable, query);
            return queryable.Count();
        }


        /// <summary>
        /// Returns a list of unqiue page URLs with the number of times they have been
        /// visited by a user.
        /// </summary>
        /// <param name="skip">Index of row to return from.</param>
        /// <param name="take">Number of rows to take.</param>
        /// <returns>List of page URLs with their view count.</returns>
        public IList<SingleStatDto> GetByPage(AnalyticsQueryModel query)
        {
            return GetGroupByItems(query, x => x.Url);
        }

        public int GetByPageCount(AnalyticsQueryModel query)
        {
            return GetGroupByCount(query, x => x.Url);
        }

        public IList<SingleStatDto> GetByTag(AnalyticsQueryModel query)
        {
            var tags = _tagService.GetTags().Select(x => new SingleStatDto { Name = x.TagName }).ToList();
            foreach (var tag in tags)
            {
                var queryable = GetQueryable();
                queryable = ApplyFilters(queryable, query);
                tag.Count = queryable.Where(x => x.Tags.Contains(tag.Name)).Count();
            }
            return tags.OrderByDescending(x => x.Count)
                .Skip(query.Skip)
                .Take(query.Take)
                .ToList();
        }

        public int GetByTagCount(AnalyticsQueryModel query)
        {
            return _tagService.GetTags().Count();
        }

        /// <summary>
        /// Returns a list of unqiue UserIdentifiers with the number of times they have been
        /// viewed a page.
        /// </summary>
        /// <param name="from">Index of row to return from.</param>
        /// <param name="to">Index of row to return up to.</param>
        /// <returns>List of UserIdentifiers with their view count.</returns>
        public IList<SingleStatDto> GetByUser(AnalyticsQueryModel query)
        {
            return GetGroupByItems(query, x => x.UserIdentifier);
        }

        public int GetByUserCount(AnalyticsQueryModel query)
        {
            return GetGroupByCount(query, x => x.UserIdentifier);
        }

        #endregion

        #region Helpers
        private IQueryable<AnalyticsEntry> ApplyFilters(IQueryable<AnalyticsEntry> queryable, AnalyticsQueryModel query)
        {
            queryable = ApplyDateFilter(queryable, query.FromUtc, query.ToUtc);
            queryable = ApplyTermFilter(queryable, query.Term);
            return queryable;
        }

        private IQueryable<AnalyticsEntry> ApplyDateFilter(IQueryable<AnalyticsEntry> queryable, DateTime? fromUtc, DateTime? toUtc)
        {
            if (fromUtc.HasValue)
                queryable = queryable.Where(x => fromUtc < x.VisitDateUtc);
            if (toUtc.HasValue)
                queryable = queryable.Where(x => toUtc > x.VisitDateUtc);
            return queryable;
        }

        private IQueryable<AnalyticsEntry> ApplyTermFilter(IQueryable<AnalyticsEntry> queryable, string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return queryable;
            return queryable.Where(x =>
               x.UserIdentifier.Contains(term) ||
               x.Url.Contains(term) ||
               (_analyticsSettings.TagsEnabled && x.Tags.Contains(term))
            );
        }

        private IQueryable<SingleStatDto> GetGroupBy(AnalyticsQueryModel query, Expression<Func<AnalyticsEntry, string>> groupSelector)
        {
            var queryable = GetQueryable();
            queryable = ApplyFilters(queryable, query);
            return queryable.GroupBy(groupSelector)
                .OrderByDescending(x => x.Count())
                .Select(x => new SingleStatDto
                {
                    Name = x.Key,
                    Count = x.Count()
                });
        }

        private int GetGroupByCount(AnalyticsQueryModel query, Expression<Func<AnalyticsEntry, string>> groupSelector)
        {
            var queryable = GetQueryable();
            queryable = ApplyFilters(queryable, query);
            return queryable.Select(groupSelector).Distinct().Count();
        }

        private IList<SingleStatDto> GetGroupByItems(AnalyticsQueryModel query, Expression<Func<AnalyticsEntry, string>> groupSelector)
        {
            var queryable = GetGroupBy(query, groupSelector)
                .Skip(query.Skip);
            if (query.Take > 0)
                queryable = queryable.Take(query.Take);
            return queryable.ToList();
        }

        private IQueryable<AnalyticsEntry> GetQueryable()
        {
            return _transactionManager.GetSession().Query<AnalyticsEntry>();
        }
        #endregion
    }
}