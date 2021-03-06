﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBType;

namespace W_ORM.MYSQL
{
    public class CSHARP_To_MYSQL : ICSHARP_To_DB
    {
        public string GetSQLQueryFormat(PropertyInfo propertyInfo)
        {
            string returnPropertyAttribute = propertyInfo.Name;
            if (propertyInfo.GetCustomAttributes().Count() > 0)
            {
                foreach (dynamic propertyAttribute in propertyInfo.GetCustomAttributes().OrderBy(x => x.TypeId))
                {
                    returnPropertyAttribute += $" {Attribute_To_SQLType(propertyAttribute)}";
                    if (propertyAttribute.AttributeName == "VARCHAR" || propertyAttribute.AttributeName == "NVARCHAR")
                        returnPropertyAttribute += $"({propertyAttribute.MaxLength}) ";

                    if (propertyAttribute.AttributeDefination == "FKey")
                        returnPropertyAttribute += $",FOREIGN KEY ({propertyAttribute.PropertyName}) REFERENCES {propertyAttribute.ClassName}({propertyAttribute.PropertyName})";

                    if (propertyAttribute.AttributeDefination == "PKey")
                        returnPropertyAttribute += $",PRIMARY KEY ({propertyInfo.Name})";
                }
            }
            else
            {
                returnPropertyAttribute += $" {PropertyType_To_SQLType(propertyInfo.PropertyType.Name)}";
            }
            return returnPropertyAttribute;
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

        /// <summary>
        /// Sınıfların propertylerine tanımlanmış olan custom attribuelerin MYSQL için gerekli olan string ifadeye döndürür
        /// Örn  : Category sınıfının CategoryID propertysine INT attribute tanımlanmış ise bu fonksiyon size int dönecektir.
        /// Örn2 : Category sınıfının CategoryID propertysine FOREIGN_KEY atrribute tanımlanmış ise bu fonksion size FOREIGN KEY dönecektir. 
        /// </summary>
        /// <param name="attribute">Attribute İsmi</param>
        /// <returns></returns>
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
                case "AUTO_INCREMENT":
                    response = "AUTO_INCREMENT"; break;

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
                    response = "INT"; break;
                case "string":
                    response = "LONGTEXT"; break;
                case "bool":
                    response = "TINYINT(1)"; break;
                case "decimal":
                    response = "DECIMAL(18,4)"; break;
                case "float":
                    response = "DECIMAL(18,4)"; break;
                case "DateTime":
                    response = "DATETIME"; break;
                default:
                    response = "LONGTEXT"; break;
            }
            return response;
        }

       
    }
}
