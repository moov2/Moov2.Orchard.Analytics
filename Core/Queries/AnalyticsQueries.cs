using Moov2.Orchard.Analytics.Models;
using Moov2.Orchard.Analytics.ViewModels.Admin;
using Orchard.Data;
using Orchard.Localization;
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
        public IList<RawAnalyticsDto> GetAll(int skip, int take)
        {

            return _repository
                .Fetch(x => true)
                .OrderByDescending(x => x.VisitDateUtc)
                .Skip(skip)
                .Take(take)
                .Select(x => new RawAnalyticsDto
                {
                    UserIdentifier = !string.IsNullOrWhiteSpace(x.UserIdentifier) ? x.UserIdentifier : T("Anonymous").ToString(),
                    Url = x.Url,
                    VisitDate = x.VisitDateUtc.ToLocalTime()
                })
                .ToList();
        }

        public int GetAllCount()
        {
            return _repository.Count(x => true);
        }


        /// <summary>
        /// Returns a list of unqiue page URLs with the number of times they have been
        /// visited by a user.
        /// </summary>
        /// <param name="skip">Index of row to return from.</param>
        /// <param name="take">Number of rows to take.</param>
        /// <returns>List of page URLs with their view count.</returns>
        public IList<ByPageAnalyticsDto> GetByPage(int skip, int take)
        {
            var result = _transactionManager.GetSession().CreateSQLQuery(ByPageSql(skip, take)).List();
            var items = new List<ByPageAnalyticsDto>();

            foreach (dynamic obj in result)
            {
                items.Add(new ByPageAnalyticsDto { Url = obj[0], Count = obj[1] });
            }

            return items;
        }

        public int GetByPageCount()
        {
            return _repository.Table.Select(x => x.Url).Distinct().Count();
        }

        /// <summary>
        /// Returns a list of unqiue UserIdentifiers with the number of times they have been
        /// viewed a page.
        /// </summary>
        /// <param name="from">Index of row to return from.</param>
        /// <param name="to">Index of row to return up to.</param>
        /// <returns>List of UserIdentifiers with their view count.</returns>
        public IList<ByUserAnalyticsDto> GetByUser(int from, int to)
        {
            var result = _transactionManager.GetSession().CreateSQLQuery(ByUserSql(from, to)).List();
            var items = new List<ByUserAnalyticsDto>();

            foreach (dynamic obj in result)
            {
                items.Add(new ByUserAnalyticsDto { User = obj[0], Count = obj[1] });
            }

            return items;
        }

        public int GetByUserCount()
        {
            return _repository.Table.Select(x => x.UserIdentifier).Distinct().Count();
        }

        #endregion

        #region Helpers
        #region SQL
        /// <summary>
        /// SQl query used to grab data about how many times pages have been
        /// visited. The from and to parameters are used to limit the number
        /// of results that are returned from the database.
        /// </summary>
        /// <param name="skip">Start index for the data to be returned.</param>
        /// <param name="take">Number of records to return.</param>
        /// <returns></returns>
        private static string ByPageSql(int skip, int take)
        {
            skip++;
            var to = skip + take;
            var sql = @"WITH OrderedAnalytics AS
                    (
                        select Url, count(Url) as 'Visits',
                        ROW_NUMBER() OVER (ORDER BY count(Url) desc) AS 'RowNumber'
                        from Moov2_Orchard_Analytics_AnalyticsEntry analyticsEntry
                        group by analyticsEntry.Url
                    ) 
                    SELECT Url, Visits
                    FROM OrderedAnalytics";

            if (take > 0)
                sql += " WHERE RowNumber BETWEEN " + skip + " AND " + to;

            sql += " ORDER BY Visits desc";

            return sql;
        }

        /// <summary>
        /// SQl query used to grab data about how many times users have viewed
        /// a page on the site. The from and to parameters are used to limit the number
        /// of results that are returned from the database.
        /// </summary>
        /// <param name="skip">Start index for the data to be returned.</param>
        /// <param name="take">Number of records to be returned.</param>
        /// <returns></returns>
        private static string ByUserSql(int skip, int take)
        {
            skip++;
            var to = skip + take;
            var sql = @"WITH OrderedAnalytics AS
                    (
                        select UserIdentifier, count(UserIdentifier) as 'Visits',
                        ROW_NUMBER() OVER (ORDER BY count(UserIdentifier) desc) AS 'RowNumber'
                        from Moov2_Orchard_Analytics_AnalyticsEntry analyticsRecord
                        group by analyticsRecord.UserIdentifier
                    ) 
                    SELECT UserIdentifier, Visits
                    FROM OrderedAnalytics";

            if (take > 0)
                sql += " WHERE RowNumber BETWEEN " + skip + " and " + to;

            sql += " ORDER BY Visits desc";

            return sql;
        }
        #endregion
        #endregion
    }
}