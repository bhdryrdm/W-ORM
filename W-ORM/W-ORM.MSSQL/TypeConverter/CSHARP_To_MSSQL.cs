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
                foreach (dynamic propertyAttribute in propertyInfo.GetCustomAttributes().OrderBy(x=> x.TypeId))
                {
                    columnAttribute += $" {Attribute_To_SQLType(propertyAttribute)}";
                    if (propertyAttribute.AttributeName == "VARCHAR" || propertyAttribute.AttributeName == "NVARCHAR")
                        columnAttribute += $"({propertyAttribute.MaxLength}) ";
                    if (propertyAttribute.AttributeDefination == "Increment")
                        columnAttribute += $"({propertyAttribute.StartNumber},{propertyAttribute.Increase}) ";
                    if (propertyAttribute.AttributeDefination == "FKey")
                        columnAttribute += $" {propertyAttribute.ClassName}({propertyAttribute.PropertyName})";
                }
            }
            else
            {
                columnAttribute += $" {PropertyType_To_SQLType(propertyInfo.PropertyType.Name)}";
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
                    column += $"{propertyAttribute.AttributeDefination}=\"{propertyAttribute.AttributeName}\" ";
                    if (propertyAttribute.AttributeName == "VARCHAR" || propertyAttribute.AttributeName == "NVARCHAR")
                        column += $"MaxLength=\"{propertyAttribute.MaxLength}\" ";
                    if (propertyAttribute.AttributeDefination == "Increment")
                        column += $"StartNumber=\"{propertyAttribute.StartNumber}\" Increase=\"{propertyAttribute.Increase}\" ";
                    if (propertyAttribute.AttributeDefination == "FKey")
                        column += $"TableName=\"{propertyAttribute.ClassName}\" ColumnName=\"{propertyAttribute.PropertyName}\" ";
                }
            }
            else
            {
                column += $"{PropertyType_To_SQLType(propertyInfo.PropertyType.Name)}";
            }
            return column + $"></{propertyInfo.Name}>";
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
                    response = "nvarchar"; break;
                case "NOTNULL":
                    response = "NOT NULL"; break;
                case "BIGINT":
                    response = "bigint"; break;
                case "BINARY":
                    response = "binary"; break;
                case "DATETIME":
                    response = "datetime"; break;
                case "DECIMAL":
                    response = "decimal"; break;
                case "MONEY":
                    response = "money"; break;
                case "SMALLINT":
                    response = "smallint"; break;
                case "TEXT":
                    response = "text"; break;
                case "TIMESTAMP":
                    response = "timestamp"; break;
                case "TINYINT":
                    response = "tinyint"; break;
                case "VARCHAR":
                    response = "varchar"; break;
                case "PRIMARY_KEY":
                    response = "PRIMARY KEY"; break;
                case "FOREIGN_KEY":
                    response = "FOREIGN KEY REFERENCES"; break;
                case "AUTO_INCREMENT":
                    response = "IDENTITY"; break;
                    
                default:

                    break;
            }
            return response;
        }
        public string PropertyType_To_SQLType(string propertyName)
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
                    response = "decimal(18,0)"; break;
                case "float":
                    response = "decimal(18,0)"; break;
                case "DateTime":
                    response = "datetime"; break;
                default:
                    response = "nvarchar(4000)"; break;
            }
            return response;
        }
    }
}
