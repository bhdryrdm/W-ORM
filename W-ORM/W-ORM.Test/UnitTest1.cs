using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Linq;
using W_ORM.Layout.DBType;
using System.Web.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace W_ORM.Test
{
    [TestClass]
    public class UnitTest1 : MSSQLProviderContext<WORMContext>
    {
        [TestMethod]
        public void TestMethod1()
        {           
            XDocument changesetDB = new XDocument(new XElement("Databases",
                                                          new XElement("WORMContext", new XAttribute("id", "WORMContext"),
                                                              new XElement("DatabaseName", new XAttribute("value", "BHDR")),
                                                              new XElement("ConnectionString", new XAttribute("value", "Server=.; Trusted_Connection=True;")),
                                                              new XElement("Provider", new XAttribute("value", "System.Data.SqlClient")),
                                                              new XElement("Type", new XAttribute("value", DBType_Enum.MSSQL)),
                                                              new XElement("Version", new XAttribute("value", 001)),
                                                              new XElement("CreatedTime", new XAttribute("value", DateTime.Now)),
                                                              new XElement("CreatedAuthor", new XAttribute("value", "Bahadır Yardım")),
                                                              new XElement("UpdatedTime", new XAttribute("value", DateTime.Now)),
                                                              new XElement("UpdatedAuthor", new XAttribute("value", "Bahadır Yardım"))),
                                                          new XElement("MYSQL", new XAttribute("id", "MYSQL"),
                                                              new XElement("ConnectionString", new XAttribute("value", "Server=.;Database=OWASP; Trusted_Connection=True;")),
                                                              new XElement("Provider", new XAttribute("value", "System.Data.Mysql")),
                                                              new XElement("Type", new XAttribute("value", DBType_Enum.MSSQL)),
                                                              new XElement("Version", new XAttribute("value", 132)),
                                                              new XElement("CreatedTime", new XAttribute("value", DateTime.Now)),
                                                              new XElement("CreatedAuthor", new XAttribute("value", "Ali Tevek")),
                                                              new XElement("UpdatedTime", new XAttribute("value", DateTime.Now)),
                                                              new XElement("UpdatedAuthor", new XAttribute("value", "Bahadır Yardım"))),
                                                          new XElement("POSTGRESQL", new XAttribute("id", "POSTGRESQL"),
                                                              new XElement("ConnectionString", new XAttribute("value", "Server=.;Database=OWASP; Trusted_Connection=True;")),
                                                              new XElement("Provider", new XAttribute("value", "System.Data.Postgre")),
                                                              new XElement("Type", new XAttribute("value", DBType_Enum.MSSQL)),
                                                              new XElement("Version", new XAttribute("value", 012)),
                                                              new XElement("CreatedTime", new XAttribute("value", DateTime.Now)),
                                                              new XElement("CreatedAuthor", new XAttribute("value", "Bahadır Yardım")),
                                                              new XElement("UpdatedTime", new XAttribute("value", DateTime.Now)),
                                                              new XElement("UpdatedAuthor", new XAttribute("value", "Ali Tevek")))));

            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            changesetDB.Save(Path.Combine(path, "WORM.config"));

            Type entityType;
            dynamic entitySchema;
            string entityColumnsMSSQL = string.Empty, entityColumnMSSQLType = string.Empty, createTableMSSQLQuery = string.Empty, 
                   entityColumnsJSON = string.Empty, entityColumnJSON = string.Empty, createJSONObject = string.Empty;
            List<dynamic> implementedEntities = (from property in EntityType.GetProperties()
                                                 from genericArguments in property.PropertyType.GetGenericArguments()
                                                 where genericArguments.BaseType.Equals(typeof(ModelBase))
                                                 select Activator.CreateInstance(genericArguments)).ToList();
            createJSONObject = "{ \"Classes\" : {";
            foreach (var entity in implementedEntities)
            {
                entityType = entity.GetType();
                entitySchema = entityType.GetCustomAttributes(typeof(SchemaAttribute), false).FirstOrDefault();

                foreach (var entityColumn in entityType.GetProperties())
                {
                    entityColumnMSSQLType = new From_CSHARP_To_MSSQL_Type().GetMSSQLFormat(entityColumn);
                    entityColumnsMSSQL += $"{entityColumnMSSQLType}, ";

                    entityColumnJSON = new From_CSHARP_To_MSSQL_Type().GetJSONFormat(entityColumn);
                    entityColumnsJSON += $"{entityColumnJSON}";
                }
                entityColumnsMSSQL = entityColumnsMSSQL.Remove(entityColumnsMSSQL.Length - 2);
                entityColumnsJSON = entityColumnsJSON.Remove(entityColumnsJSON.Length - 1);
                createTableMSSQLQuery += $"CREATE TABLE [{entitySchema.SchemaName}].[{entityType.Name}] ({entityColumnsMSSQL}) ";


                createJSONObject += $"\"{entityType.Name}\" : " +
                            "[" +
                                    $"{entityColumnsJSON}" +
                            "],";

                entityColumnsMSSQL = string.Empty;
                entityColumnsJSON = string.Empty;
            }
            createJSONObject = createJSONObject.Remove(createJSONObject.Length - 1);
            createJSONObject += "}}";
            ConnectionStringFactory.CreateDB("BHDR");
            ConnectionStringFactory.CreateSettingTable("BHDR", createJSONObject);
            ConnectionStringFactory.CreateTable("BHDR", createTableMSSQLQuery);
           
        }
         
    }
}
