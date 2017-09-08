using Orchard.UI.Resources;

namespace Moov2.Orchard.Analytics
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineScript("Analytics").SetUrl("analytics.min.js", "analytics.js");
            manifest.DefineStyle("Analytics").SetUrl("analytics.min.css", "analytics.css");
        }
    }
}