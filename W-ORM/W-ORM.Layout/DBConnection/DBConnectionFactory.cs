using System;
using System.Data.Common;
using System.IO;
using System.Xml;
using W_ORM.Layout.DBModel;

namespace W_ORM.Layout.DBConnection
{
    /// <summary>
    /// TR : Bağlantı Nesne Fabrikası
    /// EN : Connection Object Factory
    /// </summary>
    public class DBConnectionFactory
    {
        private static DbConnection dbConnection;
        private static Object objectLockControl = new Object();

        /// <summary>
        /// Private Constructor
        /// </summary>
        private DBConnectionFactory()
        {

        }

        #region TR : Ortak Bağlantı Nesnesi EN : Common Connection Object
        public static DbConnection Instance(string contextName)
        {
            if (dbConnection == null || string.IsNullOrEmpty(dbConnection.ConnectionString))
            {
                lock (objectLockControl)
                {
                    if (dbConnection == null || string.IsNullOrEmpty(dbConnection.ConnectionString))
                    {
                        var dbInformation = ReturnDBInformatinFromXML(contextName);
                        DbProviderFactory factory = DbProviderFactories.GetFactory(dbInformation.Provider);
                        dbConnection = factory.CreateConnection();
                        dbConnection.ConnectionString = $"{dbInformation.ConnectionString} Database={contextName};";
                    }
                }
            }
            return dbConnection;
        }
        #endregion

        #region TR : Veritabanı Oluşturan Bağlantı Nesnesi EN : Creating Database by Connection Object
        /// <summary>
        /// TR : WORM.config dosyasından bilgiler okunarak Bağlantı nesnesi oluşturulur
        /// EN : 
        /// </summary>
        /// <param name="contextName">WORM.config dosyasından okunacak Context Adı </param>
        /// <returns></returns>
        public static DbConnection CreateDatabaseInstance(string contextName)
        {
            var dbInformation = ReturnDBInformatinFromXML(contextName);
            DbProviderFactory factory = null;
            DbConnection connection = null;
            factory = DbProviderFactories.GetFactory(dbInformation.Provider);
            connection = factory.CreateConnection();
            connection.ConnectionString = $"{dbInformation.ConnectionString}";
            return connection;
        }
        #endregion

        #region TR : WORM.config Dosya Okuyucusu EN : WORM.config Reader
        /// <summary>
        /// TR : 
        /// EN : 
        /// </summary>
        /// <param name="contextID">TR : WORM.config dosyası içerisinden istenen ContextID </param>
        /// <returns></returns>
        public static DBInformationModel ReturnDBInformatinFromXML(string contextID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\WORM.config");
            XmlElement xmlElement = (XmlElement)xmlDoc.DocumentElement.SelectSingleNode($"{contextID}[@id='{contextID}']");
            DBInformationModel dBInformationModel = new DBInformationModel
            {
                ConnectionString = xmlElement.GetElementsByTagName("ConnectionString")[0].Attributes[0].InnerXml,
                Provider = xmlElement.GetElementsByTagName("Provider")[0].Attributes[0].InnerXml,
                UpdatedAuthor = xmlElement.GetElementsByTagName("UpdatedAuthor")[0].Attributes[0].InnerXml
            };
            return dBInformationModel;
        }

        public static int LatestDatabaseVersionFromXML(string contextID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\WORM.config");
            XmlElement xmlElement = (XmlElement)xmlDoc.DocumentElement.SelectSingleNode($"{contextID}[@id='{contextID}']");
            return Int32.Parse(xmlElement.GetElementsByTagName("Version")[0].Attributes[0].InnerXml);
        }
        #endregion

    }
}
