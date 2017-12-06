using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        private string tableName;

        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        private string schemaName;

        public string SchemaName
        {
            get { return schemaName; }
            set { schemaName = value; }
        }

        private int ordinalPosition;
        /// <summary>
        /// ForeignKey olarak tanımlanmış tabloları belirlemek için kullanılır
        /// ForeignKey olarak tanımlanmış alan hangi tabloda Primary Key ise o tablonun OrdinalPosition küçük olmalıdır
        /// </summary>
        public int OrdinalPosition
        {
            get { return ordinalPosition; }
            set { ordinalPosition = value; }
        }

    }
}
