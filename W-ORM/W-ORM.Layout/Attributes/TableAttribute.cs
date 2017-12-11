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

        /// <summary>
        /// TR : Tablo Adı
        /// EN : 
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        private string schemaName;

        /// <summary>
        /// TR : Tablo Şema Adı
        /// EN : 
        /// </summary>
        public string SchemaName
        {
            get { return schemaName; }
            set { schemaName = value; }
        }

        private int ordinalPosition;
        /// <summary>
        /// TR : ForeignKey olarak tanımlanmış tabloları belirlemek için kullanılır ForeignKey olarak tanımlanmış alan hangi tabloda Primary Key ise o tablonun OrdinalPosition küçük olmalıdır
        /// EN : 
        /// </summary>
        public int OrdinalPosition
        {
            get { return ordinalPosition; }
            set { ordinalPosition = value; }
        }

    }
}
