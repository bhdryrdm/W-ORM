using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Xml.Linq;
using W_ORM.Layout.DBType;
using W_ORM.MSSQL;

namespace W_ORM.Test
{
    [TestClass]
    public class WORMUTest
    {
        [TestMethod]
        public void SaveWormConfig()
        {   
            XDocument changesetDB = new XDocument(new XElement("Databases",
                                                          new XElement("RECEPContext", new XAttribute("id", "RECEPContext"),
                                                              new XElement("ConnectionString", new XAttribute("value", "Server=.; Trusted_Connection=True;")),
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
            changesetDB.Save(Path.Combine(path, "WORM.config"));
        }
     
    }
}
