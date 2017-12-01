using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBType;

namespace W_ORM.ORACLE
{
    public class CSHARP_To_ORACLE : ICSHARP_To_DB
    {
        public string GetSQLQueryFormat(PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }

        public string GetXMLDataFormat(PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }

        public string Attribute_To_SQLType(Attribute attribute)
        {
            throw new NotImplementedException();
        }

        public string PropertyType_To_SQLType(string propertyName)
        {
            string response = string.Empty;
            switch (propertyName)
            {
                case "int":
                    response = "NUMBER(10)"; break;
                case "string":
                    response = "NVARCHAR2(4000)"; break;
                case "bool":
                    response = "RAW(1)"; break;//NUMBER(1,0)
                case "decimal":
                    response = "FLOAT(24)"; break;
                case "float":
                    response = "NUMBER"; break;
                case "DateTime":
                    response = "DATE"; break;
                default:
                    response = "NVARCHAR2(4000)"; break;
            }
            return response;
        }
    }
}
 
