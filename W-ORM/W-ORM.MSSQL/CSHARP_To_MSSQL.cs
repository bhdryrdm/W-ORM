using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBType;

namespace W_ORM.MSSQL
{
    public class CSHARP_To_MSSQL : ICSHARP_To_DB
    {
        public string GetSQLQueryFormat(PropertyInfo propertyInfo)
        {
            string columnAttribute = propertyInfo.Name;
            if (propertyInfo.GetCustomAttributes().Count() > 0)
            {
                foreach (var propertyAttribute in propertyInfo.GetCustomAttributes())
                {
                    columnAttribute += $" {Attribute_To_SQLType(propertyAttribute)}";
                }
            }
            else
            {
                columnAttribute += $" {PropertyName_To_SQLType(propertyInfo.PropertyType.Name)}";
            }
            return columnAttribute;
        }

        public string GetXMLDataFormat(PropertyInfo propertyInfo)
        {
            string column = $"<{propertyInfo.Name} ";
            if (propertyInfo.GetCustomAttributes().Count() > 0)
            {
                foreach (dynamic propertyAttribute in propertyInfo.GetCustomAttributes())
                {
                    column += $"{propertyAttribute.AttributeDefination}={Attribute_To_SQLType(propertyAttribute)} ";
                }
            }
            else
            {
                column += $"{PropertyName_To_SQLType(propertyInfo.PropertyType.Name)}";
            }
            return column + $" ><{propertyInfo.Name}/>";
        }

        public string Attribute_To_SQLType(Attribute attribute)
        {
            string response = string.Empty;
            switch (attribute.GetType().Name)
            {
                case "INT":
                    response = "int"; break;
                case "BIT":
                    response = "bit"; break;
                case "NVARCHAR":
                    response = "nvarchar(4000)"; break;
                case "NOTNULL":
                    response = "NOT NULL"; break;
                default:

                    break;
            }
            return response;
        }
        public string PropertyName_To_SQLType(string propertyName)
        {
            string response = string.Empty;
            switch (propertyName)
            {
                case "int":
                    response = "int"; break;
                case "string":
                    response = "nvarchar(4000)"; break;
                case "bool":
                    response = "bit"; break;
                case "decimal":
                    response = "decimal"; break;
                case "DateTime":
                    response = "datetime"; break;
                case "float":
                    response = "float"; break;
                default:
                    response = "nvarchar(4000)"; break;
            }
            return response;
        }
    }
}
