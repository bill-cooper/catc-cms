using System;
using System.Data;
using Orchard.Data.Migration.Schema;

namespace ceenq.com.Core.Data.Migration
{
    public static class SchemaBuilderHelper
    {
        /// <summary>
        /// Abstracts the creating of the Orchard create table commands using convention based on the record class metadata.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sb">The Schema Builder.</param>
        /// <returns></returns>
        public static SchemaBuilder CreateFor<T>(this SchemaBuilder sb)
        {
            var t = typeof (T);
            return sb.CreateTable(t.Name, table =>
                {
                    table = table.ContentPartRecord();
                    foreach (var prop in t.GetProperties())
                    {
                        if(prop.DeclaringType != t) continue;
                        table = table.Column(prop.Name, prop.PropertyType.DbType());
                    }
                }
                );
        }

        private static DbType DbType(this Type t)
        {
            if (t.IsGenericType)// must be Nullable<T>
                t = t.GetGenericArguments()[0];

            return SchemaUtils.ToDbType(t);
        }

    }
}
