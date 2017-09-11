using Moov2.Orchard.Analytics.ViewModels.Admin;
using Orchard;
using System.Collections.Generic;

namespace Moov2.Orchard.Analytics.Core.Queries
{
    public interface IAnalyticsQueries : IDependency
    {
        IList<RawAnalyticsDto> GetAll(AnalyticsQueryModel query);
        int GetAllCount(AnalyticsQueryModel query);
        IList<ByPageAnalyticsDto> GetByPage(AnalyticsQueryModel query);
        int GetByPageCount(AnalyticsQueryModel query);
        IList<ByUserAnalyticsDto> GetByUser(AnalyticsQueryModel query);
        int GetByUserCount(AnalyticsQueryModel query);
    }
}
