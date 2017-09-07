using System;

namespace Moov2.Orchard.Analytics.Models
{
    public class AnalyticsEntry
    {
        public virtual int Id { get; set; }
        public virtual string UserIdentifier { get; set; }
        public virtual string Url { get; set; }
        public virtual DateTime VisitDateUtc { get; set; }
    }
}