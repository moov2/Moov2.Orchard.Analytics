namespace Moov2.Orchard.Analytics.ViewModels.Admin
{
    public class ByUserAnalyticsViewModel : AnalyticsViewModel<ByUserAnalyticsDto>
    {
    }

    public class ByUserAnalyticsDto
    {
        public string User { get; set; }
        public int Count { get; set; }
    }
}