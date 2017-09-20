using Moov2.Orchard.Analytics.Core.Settings;
using Orchard;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Moov2.Orchard.Analytics
{
    public class AdminMenu : INavigationProvider
    {
        #region Dependencies
        private readonly IAnalyticsSettings _analyticsSettings;

        public IOrchardServices Services { get; set; }

        public Localizer T { get; set; }
        #endregion

        #region Constructor
        public AdminMenu(IAnalyticsSettings analyticsSettings, IOrchardServices services)
        {
            T = NullLocalizer.Instance;
            Services = services;

            _analyticsSettings = analyticsSettings;
        }
        #endregion

        #region INavigationProvider
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            if (!Services.Authorizer.Authorize(Permissions.ViewAnalytics))
                return;

            builder.AddImageSet("analytics")
                .Add(T("Analytics"), "9", menu =>
                {
                    var action = menu.Action("Index", "Admin", new { area = "Moov2.Orchard.Analytics" })
                        .Add(T("Raw"), "0", item => item.Action("Index", "Admin", new { area = "Moov2.Orchard.Analytics" }).LocalNav())
                        .Add(T("By Page"), "1", item => item.Action("ByPage", "Admin", new { area = "Moov2.Orchard.Analytics" }).LocalNav())
                        .Add(T("By User"), "2", item => item.Action("ByUser", "Admin", new { area = "Moov2.Orchard.Analytics" }).LocalNav());

                    if (_analyticsSettings.TagsEnabled)
                        action.Add(T("By Tag"), "3", item => item.Action("ByTag", "Admin", new { area = "Moov2.Orchard.Analytics" }).LocalNav());
                });
        }
        #endregion
    }
}