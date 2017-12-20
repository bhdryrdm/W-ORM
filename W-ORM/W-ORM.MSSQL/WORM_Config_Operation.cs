using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using W_ORM.Layout.DBType;

namespace W_ORM.MSSQL
{
    public static class WORM_Config_Operation
    {
        public static string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\WORM.config";

        /// <summary>
        /// WORM.config dosyasına yeni context veritabanı eklemek için yada güncellemek kullanılır
        /// </summary>
        /// <typeparam name="TContext">Context Veritabanı Adı</typeparam>
        /// <param name="connectionString">Context(Veritabanı) bağlantı cümlesi</param>
        /// <param name="dBType_Enum">Veritabanı tipi</param>
        /// <param name="author">Yazar ismi</param>
        public static void SaveWormConfig<TContext>(string connectionString, DBType_Enum dBType_Enum, string author)
        {
            #region WORM.config dosyası var mı kontrolü yapılır
            if (!File.Exists(path))
            {
                File.Create(path);
                new XDocument(new XElement("Databases")).Save(path);
            }
            #endregion

            #region WORM.config dosyasına yeni context oluşturulur yada üzerine yazılır
            string contextName = typeof(TContext).Name;
            XDocument xDocument = XDocument.Load(path);
            if (xDocument.Element("Databases").Elements(contextName) != null)
                xDocument.Element("Databases").Elements(contextName).Remove();

            XElement contextRoot = new XElement(contextName);
            contextRoot.Add(new XAttribute("id", contextName));
            contextRoot.Add(new XElement("ConnectionString", new XAttribute("value", connectionString)),
                            new XElement("Provider", new XAttribute("value", DBType_To_DBProvider.GetProviderFactoryByEnum(dBType_Enum))),
                            new XElement("UpdatedAuthor", new XAttribute("value", author)));

            xDocument.Element("Databases").Add(contextRoot);
            xDocument.Save(path);
            #endregion

            #region MSSQL için Veritabanı, Tablo ve Sütunlar oluşturulur
            CreateEverythingForMSSQL<TContext>();
            #endregion
        }

        private static void CreateEverythingForMSSQL<TContext>()
        {
            #region MSSQL için oluşturulacak sorgular oluşturulur
            CreateDatabase<TContext> createDatabase = new CreateDatabase<TContext>();
            Tuple<string, string> tupleData = createDatabase.EntityClassQueries();

            DB_Operation dB_Operation = new DB_Operation(typeof(TContext).Name);
            Tuple<bool,int> tupleData2 = dB_Operation.CreateORAlterDatabaseAndTables(tupleData.Item2, tupleData.Item1);
            #endregion

            #region Veritabanı versiyon bilgisine ulaşılır ve WORM.config dosyasına yazılır
            string contextName = typeof(TContext).Name;
            XDocument xDocument = XDocument.Load(path);
            if (xDocument.Element("Databases").Element(contextName).Elements("Version") != null)
                xDocument.Element("Databases").Element(contextName).Elements("Version").Remove();
            xDocument.Element("Databases").Element(contextName).Add(new XElement("Version", new XAttribute("value", tupleData2.Item2)));
            xDocument.Save(path);
            #endregion
        }

        public static void CreateContext<TContext>(int dbVersion, string contextPath = "", string namespaceName = "")
        {
            string contextName = typeof(TContext).Name;
            DB_Operation dB_Operation = new DB_Operation(contextName);
            dB_Operation.ContextGenerateFromDB(dbVersion, contextPath, namespaceName);
            CreateEverythingForMSSQL<TContext>();
        }
    }
}
