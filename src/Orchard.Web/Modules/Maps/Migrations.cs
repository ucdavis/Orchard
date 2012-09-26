using System;
using System.Collections.Generic;
using System.Data;
using Maps.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Maps {
    public class Migrations : DataMigrationImpl {

        public int Create() {
			// Creating table MapRecord
			SchemaBuilder.CreateTable("MapRecord", table => table
				.ContentPartRecord()
				.Column("Latitude", DbType.Double)
				.Column("Longitude", DbType.Double)
			);

            ContentDefinitionManager.AlterPartDefinition(
                typeof(MapPart).Name, cfg => cfg.Attachable());

            return 1;
        }

        public int UpdateFrom1()
        {
            // Create a new widget content type with our map
            ContentDefinitionManager.AlterTypeDefinition("MapWidget", cfg => cfg
                .WithPart("MapPart")
                .WithPart("WidgetPart")
                .WithPart("CommonPart")
                .WithSetting("Stereotype", "Widget"));

            return 2;
        }
    }
}