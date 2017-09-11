using Moov2.Orchard.Analytics.ViewModels.Admin;
using Orchard;
using System.Collections.Generic;

namespace Moov2.Orchard.Analytics.Core.Queries
{
    public interface IAnalyticsQueries : IDependency
    {
        IList<RawAnalyticsDto> GetAll(int skip, int take);
        int GetAllCount();
        IList<ByPageAnalyticsDto> GetByPage(int skip, int take);
        int GetByPageCount();
        IList<ByUserAnalyticsDto> GetByUser(int skip, int take);
        int GetByUserCount();
    }
}
