using System.Collections.Generic;

namespace Moov2.Orchard.Analytics.ViewModels.Admin
{
    public class ByUserAnalyticsViewModel
    {
        public IList<ByUserAnalyticsDto> Entries { get; set; }

        public dynamic Pager { get; set; }
    }

    public class ByUserAnalyticsDto
    {
        public string User { get; set; }
        public int Count { get; set; }
    }
}