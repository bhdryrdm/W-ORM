using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Test
{
    public class From_CSHARP_To_MSSQL_Type
    {
        public From_CSHARP_To_MSSQL_Type()
        {

        }
        public string GetMSSQLFormat(PropertyInfo propertyType)
        {
            string columnAttribute = propertyType.Name;
            if(propertyType.GetCustomAttributes().Count() > 0)
            {
                foreach (var propertyAttribute in propertyType.GetCustomAttributes())
                {
                    columnAttribute += $" {ReturnFromAttributeToMSSQLType(propertyAttribute)} ";
                }
            }
            else
            {
                columnAttribute += $" {ReturnFromPropertTypeToMSSQLType(propertyType.PropertyType.Name)} ";
            }
            return columnAttribute;
        }
        public string GetJSONFormat(PropertyInfo propertyType)
        {
            string columnName = "{\"ColumnName\" : " + $"\"{propertyType.Name}\",";
            string columnAttributes = "\"ColumnAttributes\" : \"";
            if (propertyType.GetCustomAttributes().Count() > 0)
            {
                foreach (var propertyAttribute in propertyType.GetCustomAttributes())
                {
                    columnAttributes += $"{ReturnFromAttributeToMSSQLType(propertyAttribute)}, ";
                }
            }
            else
            {
                columnAttributes += $"{ReturnFromPropertTypeToMSSQLType(propertyType.PropertyType.Name)}, ";
            }
            columnAttributes = columnAttributes.Remove(columnAttributes.Length - 2);
            return columnName + columnAttributes + "\"},";
        }
        public string ReturnFromAttributeToMSSQLType(Attribute attribute)
        {
            string response = string.Empty;
            switch (attribute.GetType().Name)
            {
                case "INT" :
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
        public string ReturnFromPropertTypeToMSSQLType(string propertyType)
        {
            string response = string.Empty;
            switch (propertyType)
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
