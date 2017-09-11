using System.Collections.Generic;

namespace Moov2.Orchard.Analytics.ViewModels.Admin
{
    public class ByPageAnalyticsViewModel
    {
        public IList<ByPageAnalyticsDto> Entries { get; set; }

        public dynamic Pager { get; set; }
    }

    public class ByPageAnalyticsDto
    {
        public string Url { get; set; }
        public int Count { get; set; }
    }
}