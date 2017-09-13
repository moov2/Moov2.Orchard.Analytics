using Orchard.Core.Common.ViewModels;
using System.Collections.Generic;

namespace Moov2.Orchard.Analytics.ViewModels.Admin
{
    public class AnalyticsViewModel<T> : AnalyticsViewModel
    {
        public IList<T> Entries { get; set; }
    }

    public class AnalyticsViewModel
    {
        public AnalyticsViewModel()
        {
            DownloadCsv = false;
            TagsEnabled = false;
        }

        public bool DownloadCsv { get; set; }

        public DateTimeEditor From { get; set; }
        public DateTimeEditor To { get; set; }

        public dynamic Pager { get; set; }

        public bool TagsEnabled { get; set; }

        public string Term { get; set; }
    }
}