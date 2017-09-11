using System;
using System.Collections.Generic;

namespace Moov2.Orchard.Analytics.ViewModels.Admin
{
    public class RawAnalyticsViewModel
    {
        public IList<RawAnalyticsDto> Entries { get; set; }
        public dynamic Pager { get; set; }
    }

    public class RawAnalyticsDto
    {
        public string Url { get; set; }
        public string UserIdentifier { get; set; }
        public DateTime? VisitDate { get; set; }
    }
}