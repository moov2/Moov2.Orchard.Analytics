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
                    .Column<int>("ContentItemId")
                    .Column<string>("Tags", c => c.WithLength(int.MaxValue))
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

            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("AnalyticsEntry",
                table => table
                    .AddColumn<int>("ContentItemId")
            );

            SchemaBuilder.AlterTable("AnalyticsEntry",
                table => table
                    .AddColumn<string>("Tags", c => c.WithLength(int.MaxValue))
            );

            return 2;
        }
    }
}