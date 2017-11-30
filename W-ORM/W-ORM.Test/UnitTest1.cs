using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Xml.Linq;
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
<<<<<<< HEAD
                                                          new XElement("WORMContext", new XAttribute("id", "WORMContext"),
                                                              new XElement("DatabaseName", new XAttribute("value", "BHDR")),
                                                              new XElement("ConnectionString", new XAttribute("value", "Server=DESKTOP-OC11CCT\\SQL_2014; Trusted_Connection=True;")),
=======
                                                          new XElement("RECEPContext", new XAttribute("id", "RECEPContext"),
                                                              new XElement("ConnectionString", new XAttribute("value", "Server=.; Trusted_Connection=True;")),
>>>>>>> f39a5d4865cbce928b573d39dbf3bd842d2306be
                                                              new XElement("Provider", new XAttribute("value", "System.Data.SqlClient")),
                                                              new XElement("Type", new XAttribute("value", DBType_Enum.MSSQL)),
                                                              new XElement("UpdatedTime", new XAttribute("value", DateTime.Now)),
                                                              new XElement("UpdatedAuthor", new XAttribute("value", "Bahadır Yardım"))),
                                                          new XElement("MYSQL-BHDR", new XAttribute("id", "MYSQL-BHDR"),
                                                              new XElement("ConnectionString", new XAttribute("value", "Server=127.0.0.1;Port=3306;Uid=root;Pwd=123qwe;")),
                                                              new XElement("Provider", new XAttribute("value", "System.Data.OleDb")),
                                                              new XElement("Type", new XAttribute("value", DBType_Enum.MYSQL)),
                                                              new XElement("UpdatedTime", new XAttribute("value", DateTime.Now)),
                                                              new XElement("UpdatedAuthor", new XAttribute("value", "Bahadır Yardım"))),
                                                          new XElement("POSTGRESQL", new XAttribute("id", "POSTGRESQL"),
                                                              new XElement("ConnectionString", new XAttribute("value", "Server=.;Database=OWASP; Trusted_Connection=True;")),
                                                              new XElement("Provider", new XAttribute("value", "System.Data.Postgre")),
                                                              new XElement("Type", new XAttribute("value", DBType_Enum.MSSQL)),
                                                              new XElement("UpdatedTime", new XAttribute("value", DateTime.Now)),
                                                              new XElement("UpdatedAuthor", new XAttribute("value", "Ali Tevek")))));

            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
           // changesetDB.Save(Path.Combine(path, "WORM.config"));
            
            //DB_Operation dB_Operation = new DB_Operation("RECEPContext");
           
            //dB_Operation.ContextGenerateFromDB(1);


            CreateEverything<RECEPContext> cr = new CreateEverything<RECEPContext>();

           

            RECEPContext wm = new RECEPContext();
            Category category = new Category { CategoryID = 1, IsActive = true, ProductName = "Test" };
            wm.Category.Insert(category);

            wm.PushToDB("RECEPContext");

           
        }
         
    }
}
