using System;

namespace Moov2.Orchard.Analytics.Core.Queries
{
    public class AnalyticsQueryModel
    {
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Term { get; set; }
    }
}