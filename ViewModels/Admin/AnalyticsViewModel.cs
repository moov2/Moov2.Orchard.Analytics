using Orchard.Core.Common.ViewModels;
using System.Collections.Generic;

namespace Moov2.Orchard.Analytics.ViewModels.Admin
{
    public abstract class AnalyticsViewModel<T> : AnalyticsViewModel
    {
        public IList<T> Entries { get; set; }
    }

    public abstract class AnalyticsViewModel
    {
        public DateTimeEditor From { get; set; }
        public DateTimeEditor To { get; set; }

        public dynamic Pager { get; set; }
    }
}