namespace Moov2.Orchard.Analytics.ViewModels.Admin
{
    public class ByPageAnalyticsViewModel : AnalyticsViewModel<ByPageAnalyticsDto>
    {
    }

    public class ByPageAnalyticsDto
    {
        public string Url { get; set; }
        public int Count { get; set; }
    }
}