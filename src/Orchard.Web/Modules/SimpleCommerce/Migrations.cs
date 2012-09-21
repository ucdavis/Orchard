using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Indexing;

namespace SimpleCommerce {
    public class Migrations : DataMigrationImpl {

        public int Create() {
			// Creating table ProductPartRecord
			SchemaBuilder.CreateTable("ProductPartRecord", table => table
				.ContentPartRecord()
				.Column("Sku", DbType.String)
				.Column("Price", DbType.Single)
			);



            return 1;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterPartDefinition("ProductPart",
              builder => builder.Attachable());
            return 2;
        }

        public int UpdateFrom2()
        {
            ContentDefinitionManager.AlterTypeDefinition("Product", cfg => cfg
              .WithPart("CommonPart")
              .WithPart("RoutePart")
              .WithPart("BodyPart")
              .WithPart("ProductPart")
              .WithPart("CommentsPart")
              .WithPart("TagsPart")
              .WithPart("LocalizationPart")
              .Creatable()
              .Indexed());
            return 3;
        }

    }
}