using Moov2.Orchard.Analytics.ViewModels.Admin;
using Moov2.Orchard.Analytics.ViewModels.Admin.Dto;
using Orchard;
using System.Collections.Generic;

namespace Moov2.Orchard.Analytics.Core.Queries
{
    public interface IAnalyticsQueries : IDependency
    {
        IList<RawAnalyticsDto> GetAll(AnalyticsQueryModel query);
        int GetAllCount(AnalyticsQueryModel query);
        IList<SingleStatDto> GetByPage(AnalyticsQueryModel query);
        int GetByPageCount(AnalyticsQueryModel query);
        IList<SingleStatDto> GetByTag(AnalyticsQueryModel query);
        int GetByTagCount(AnalyticsQueryModel query);
        IList<SingleStatDto> GetByUser(AnalyticsQueryModel query);
        int GetByUserCount(AnalyticsQueryModel query);
    }
}
