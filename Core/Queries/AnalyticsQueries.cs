using Moov2.Orchard.Analytics.Models;
using Moov2.Orchard.Analytics.ViewModels.Admin;
using NHibernate.Linq;
using Orchard.Data;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moov2.Orchard.Analytics.Core.Queries
{
    public class AnalyticsQueries : IAnalyticsQueries
    {
        #region Dependencies
        private readonly IRepository<AnalyticsEntry> _repository;
        private readonly ITransactionManager _transactionManager;

        public Localizer T { get; set; }
        #endregion

        #region Constructor
        public AnalyticsQueries(IRepository<AnalyticsEntry> repository, ITransactionManager transactionManager)
        {
            T = NullLocalizer.Instance;

            _repository = repository;
            _transactionManager = transactionManager;
        }
        #endregion

        #region IAnalyticsQueries
        public IList<RawAnalyticsDto> GetAll(AnalyticsQueryModel query)
        {
            var queryable = GetQueryable();
            queryable = ApplyDateFilter(queryable, query.FromUtc, query.ToUtc);
            queryable = queryable.OrderByDescending(x => x.VisitDateUtc)
                .Skip(query.Skip);
            if (query.Take > 0)
                queryable = queryable.Take(query.Take);
            return queryable
                .Select(x => new RawAnalyticsDto
                {
                    UserIdentifier = !string.IsNullOrWhiteSpace(x.UserIdentifier) ? x.UserIdentifier : T("Anonymous").ToString(),
                    Url = x.Url,
                    VisitDate = x.VisitDateUtc.ToLocalTime()
                }).ToList();
        }

        public int GetAllCount(AnalyticsQueryModel query)
        {
            var queryable = GetQueryable();
            queryable = ApplyDateFilter(queryable, query.FromUtc, query.ToUtc);
            return queryable.Count();
        }


        /// <summary>
        /// Returns a list of unqiue page URLs with the number of times they have been
        /// visited by a user.
        /// </summary>
        /// <param name="skip">Index of row to return from.</param>
        /// <param name="take">Number of rows to take.</param>
        /// <returns>List of page URLs with their view count.</returns>
        public IList<ByPageAnalyticsDto> GetByPage(AnalyticsQueryModel query)
        {
            var queryable = GetQueryable();
            queryable = ApplyDateFilter(queryable, query.FromUtc, query.ToUtc);
            var dtoQueryable = queryable.GroupBy(x => x.Url)
                .OrderByDescending(x => x.Count())
                .Select(x => new ByPageAnalyticsDto
                {
                    Url = x.Key,
                    Count = x.Count()
                })
                .Skip(query.Skip);
            if (query.Take > 0)
                dtoQueryable = dtoQueryable.Take(query.Take);
            return dtoQueryable.ToList();
        }

        public int GetByPageCount(AnalyticsQueryModel query)
        {
            var queryable = GetQueryable();
            queryable = ApplyDateFilter(queryable, query.FromUtc, query.ToUtc);
            return queryable.Select(x => x.Url).Distinct().Count();
        }

        /// <summary>
        /// Returns a list of unqiue UserIdentifiers with the number of times they have been
        /// viewed a page.
        /// </summary>
        /// <param name="from">Index of row to return from.</param>
        /// <param name="to">Index of row to return up to.</param>
        /// <returns>List of UserIdentifiers with their view count.</returns>
        public IList<ByUserAnalyticsDto> GetByUser(AnalyticsQueryModel query)
        {
            var queryable = GetQueryable();
            queryable = ApplyDateFilter(queryable, query.FromUtc, query.ToUtc);
            var dtoQueryable = queryable.GroupBy(x => x.UserIdentifier)
            .OrderByDescending(x => x.Count())
            .Select(x => new ByUserAnalyticsDto
            {
                User = x.Key,
                Count = x.Count()
            })
            .Skip(query.Skip);
            if (query.Take > 0)
                dtoQueryable = dtoQueryable.Take(query.Take);
            return dtoQueryable.ToList();
        }

        public int GetByUserCount(AnalyticsQueryModel query)
        {
            var queryable = GetQueryable();
            queryable = ApplyDateFilter(queryable, query.FromUtc, query.ToUtc);
            return queryable.Select(x => x.UserIdentifier).Distinct().Count();
        }

        #endregion

        #region Helpers
        private IQueryable<AnalyticsEntry> ApplyDateFilter(IQueryable<AnalyticsEntry> queryable, DateTime? fromUtc, DateTime? toUtc)
        {
            if (fromUtc.HasValue)
                queryable = queryable.Where(x => fromUtc < x.VisitDateUtc);
            if (toUtc.HasValue)
                queryable = queryable.Where(x => toUtc > x.VisitDateUtc);
            return queryable;
        }

        private IQueryable<AnalyticsEntry> GetQueryable()
        {
            return _transactionManager.GetSession().Query<AnalyticsEntry>();
        }
        #endregion
    }
}