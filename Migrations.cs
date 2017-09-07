using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using System;

namespace Moov2.Orchard.Analytics
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("AnalyticsEntry",
                table => table
                    .Column<int>("Id", c => c.Identity().PrimaryKey())
                    .Column<string>("UserIdentifier")
                    .Column<string>("Url")
                    .Column<DateTime>("VisitDateUtc", c => c.NotNull())
            );

            ContentDefinitionManager.AlterPartDefinition("AnalyticsPart", cfg => cfg
                .WithDescription("Adds analytics code to the page for tracking page views.")
                .Attachable());

            ContentDefinitionManager.AlterTypeDefinition("AnalyticsWidget", cfg => cfg
                .WithPart("WidgetPart")
                .WithPart("CommonPart")
                .WithPart("AnalyticsPart")
                .WithSetting("Stereotype", "Widget")
            );

            return 1;
        }
    }
}