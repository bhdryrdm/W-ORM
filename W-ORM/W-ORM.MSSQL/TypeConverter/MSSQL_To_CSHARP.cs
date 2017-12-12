using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBType;

namespace W_ORM.MSSQL
{
    public class MSSQL_To_CSHARP : IDB_To_CSHARP
    {
        private static Type propertyType;

        public static Type PropertyType
        {
            get { return propertyType; }
            set { propertyType = value; }
        }

        public Type XML_To_CSHARP(string xmlType)
        {
            switch (xmlType)
            {
                case "INT":
                    propertyType = typeof(Int32); break;
                case "BIGINT":
                    propertyType = typeof(Int64); break;
                case "BINARY":
                    propertyType = typeof(Byte); break;
                case "BIT":
                    propertyType = typeof(Boolean); break;
                case "DATETIME":
                    propertyType = typeof(DateTime); break;
                case "MONEY":
                    propertyType = typeof(Decimal); break;
                case "SMALLINT":
                    propertyType = typeof(Int16); break;
                case "TEXT":
                    propertyType = typeof(String); break;
                case "TIMESTAMP":
                    propertyType = typeof(DateTime); break;
                case "TINYINT":
                    propertyType = typeof(Byte); break;
                case "NVARCHAR":
                    propertyType = typeof(String); break;
                case "VARCHAR":
                    propertyType = typeof(String); break;
                case "DECIMAL":
                    propertyType = typeof(Decimal); break;
                default:
                    break;
            }
            return propertyType;
        }
    }
}
