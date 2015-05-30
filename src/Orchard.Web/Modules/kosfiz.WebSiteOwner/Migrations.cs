using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace kosfiz.WebSiteOwner {
    public class Migrations : DataMigrationImpl {

        public int Create() {

            SchemaBuilder.CreateTable("WebSiteOwnerRecord", table => table.Column("Id", DbType.Int32, column => column.PrimaryKey().Identity())
                .Column("Title", DbType.String, column=>column.NotNull())
                .Column("MetaName", DbType.String, column=>column.NotNull())
                .Column("MetaContent", DbType.String, column=>column.NotNull()));

            return 1;
        }
    }
}