using System;

namespace Moov2.Orchard.Analytics.ViewModels.Admin.Dto
{
    public class RawAnalyticsDto
    {
        public string Url { get; set; }
        public string UserIdentifier { get; set; }
        public DateTime? VisitDate { get; set; }
        public string Tags { get; set; }
    }
}