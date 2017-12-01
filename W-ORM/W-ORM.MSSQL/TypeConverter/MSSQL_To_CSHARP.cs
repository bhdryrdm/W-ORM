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
                case "int":
                    propertyType = typeof(Int32); break;
                case "string":
                    propertyType = typeof(String); break;
                case "decimal":
                    propertyType = typeof(Decimal); break;
                default:
                    break;
            }
            return propertyType;
        }
    }
}
