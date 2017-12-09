using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using W_ORM.Layout.DBType;
using W_ORM.MSSQL;

namespace W_ORM.Test.MSSQL
{
    [TestClass]
    public class MSSQL_Unit_Tests
    {
        public static string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        [TestMethod]
        public void GetList()
        {
            University university = new University();
            university.Department.Insert(new Entities.Department { DepartmentID = 1, DepartmentName = "Test" });
            university.PushToDB();

            var departmentList = university.Department.ToList();
            var student = university.Student.FirstOrDefault(x => x.StudentID == 1 && x.StudentName == "Bahadır" || x.StudentSurName == "Yardım");
        }

        [TestMethod]
        public void ContextGenerateFromDB()
        {
            DB_Operation dB_Operation = new DB_Operation(typeof(University).Name);
            dB_Operation.ContextGenerateFromDB(1,"","","BHDR_Context");
        }



        [TestMethod]
        public void CreateEverythingForMSSQL()
        {
            CreateEverything<University> createEverything = new CreateEverything<University>();
            Tuple<string,string> tupleData = createEverything.EntityClassQueries();

            DB_Operation dB_Operation = new DB_Operation(typeof(University).Name);
            dB_Operation.CreateORAlterDatabaseAndTables(tupleData.Item2,tupleData.Item1);
        }

        [TestMethod]
        public void CreateContextWormConfig()
        {
            SaveWormConfig<University>("Server =.; Trusted_Connection = True;", DBType_Enum.MSSQL, "bhdryrdm");
        }
        /// <summary>
        /// WORM.config dosyasına yeni context veritabanı eklemek için kullanılır
        /// </summary>
        /// <typeparam name="TContext">Context Veritabanı Adı</typeparam>
        /// <param name="connectionString">Context(Veritabanı) bağlantı cümlesi</param>
        /// <param name="dBType_Enum">Veritabanı tipi</param>
        /// <param name="author">Yazar ismi</param>
        public static void SaveWormConfig<TContext>(string connectionString, DBType_Enum dBType_Enum, string author)
        {
            #region WORM.config dosyası var mı kontrolü yapılır
            path += "\\WORM.config";
            if (!File.Exists(path))
            {
                using (File.Create(path)) { };
                new XDocument(new XElement("Databases")).Save(path);

            }
            #endregion

            #region WORM.config dosyasından tüm contextler okunur
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            List<string> allContext = new List<string>();
            foreach (XmlNode item in xmlDocument.GetElementsByTagName("Databases"))
            {
                foreach (XmlNode context in item.ChildNodes)
                {
                    allContext.Add(context.Name);
                }
            }
            #endregion

            #region WORM.config dosyasına yeni context okunur
            string contextName = typeof(TContext).Name;
            if (allContext.FirstOrDefault(x => x == contextName) == null)
            {
                XDocument xDocument = XDocument.Load(path);
                XElement contextRoot = new XElement(contextName);
                contextRoot.Add(new XAttribute("id", contextName));
                contextRoot.Add(new XElement("ConnectionString", new XAttribute("value", connectionString)),
                                new XElement("Provider", new XAttribute("value", GetProviderFactoryByEnum(dBType_Enum))),
                                new XElement("UpdatedAuthor", new XAttribute("value", author)));


                xDocument.Element("Databases").Add(contextRoot);
                xDocument.Save(path);
            }
            else
            {
                throw new Exception("Eklenmek istenen " + contextName + " contexti WORM.Config dosyasında zaten bulunmaktadır.Lütfen farklı bir context ismi belirleyiniz!");
            }
            #endregion
        }

        public static string GetProviderFactoryByEnum(DBType_Enum dBType_Enum)
        {
            string returnValue = string.Empty;
            switch (dBType_Enum)
            {
                case DBType_Enum.MSSQL:
                    returnValue = "System.Data.SqlClient"; break;
                case DBType_Enum.MYSQL:
                    returnValue = "MySql.Data.MySqlClient"; break;
                default: break;
            }
            return returnValue;
        }
    }
}
