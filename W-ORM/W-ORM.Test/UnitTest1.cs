using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBType;
using W_ORM.MSSQL;

namespace W_ORM.Test
{
    [TestClass]
    public class UnitTest1
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
                                                              new XElement("DatabaseName", new XAttribute("value", "BHDR")),
                                                              new XElement("ConnectionString", new XAttribute("value", "Server=127.0.0.1;Port=3306;Uid=root;Pwd=123qwe;")),
                                                              new XElement("Provider", new XAttribute("value", "MySql.Data.MySqlClient")),
                                                              new XElement("Type", new XAttribute("value", DBType_Enum.MYSQL)),
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


            CreateEverything<WORMContext> cr = new CreateEverything<WORMContext>("KEMAL");
            
            WORMContext wm = new WORMContext();
            Category category = new Category { CategoryID = 1, IsActive = true, ProductName = "Test" };
            wm.Category.Insert(category);

            wm.PushToDB("WORMContext");

            Product product = new Product();
            wm.Product.Insert(product);
           
        }
         
    }
}
