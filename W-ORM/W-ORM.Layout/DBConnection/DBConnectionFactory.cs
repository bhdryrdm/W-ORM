using System;
using System.Data.Common;
using System.IO;
using System.Xml;
using W_ORM.Layout.DBModel;

namespace W_ORM.Layout.DBConnection
{
    public class DBConnectionFactory
    {
        private static DbConnection dbConnectionString;
        private static Object objectLockControl = new Object();
        private static Object createDatabseLockControl = new Object();

        private DBConnectionFactory()
        {

        }

        #region I.Veritabanı yada genel olarak kullanım için gerekli olan kod bloğu
        public static DbConnection Instance(string contextName = "")
        {
            if (dbConnectionString == null)
            {
                lock (objectLockControl)
                {
                    if (dbConnectionString == null)
                    {
                        var dbInformation = ReturnDBInformatinFromXML(contextName);
                        DbProviderFactory factory = DbProviderFactories.GetFactory(dbInformation.Provider);
                        DbConnection connection = factory.CreateConnection();
                        connection.ConnectionString = $"{dbInformation.ConnectionString}";
                        if (!String.IsNullOrEmpty(contextName))
                        {
                            connection.ConnectionString += $" Database={contextName};";
                        }
                        return connection;
                    }
                }
            }
            return dbConnectionString;
        }
        #endregion

        public static DbConnection CreateDatabaseInstance(string contextName)
        {
            lock (createDatabseLockControl)
            {
                var dbInformation = ReturnDBInformatinFromXML(contextName);
                DbProviderFactory factory = null;
                DbConnection connection = null;
                factory = DbProviderFactories.GetFactory(dbInformation.Provider);
                connection = factory.CreateConnection();
                connection.ConnectionString = $"{dbInformation.ConnectionString}";
                return connection;
            }
        }

        public static DBInformationModel ReturnDBInformatinFromXML(string contextID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\WORM.config");
            XmlElement xmlElement = (XmlElement)xmlDoc.DocumentElement.SelectSingleNode($"{contextID}[@id='{contextID}']");
            DBInformationModel dBInformationModel = new DBInformationModel
            {
                ConnectionString = xmlElement.GetElementsByTagName("ConnectionString")[0].Attributes[0].InnerXml,
                Provider = xmlElement.GetElementsByTagName("Provider")[0].Attributes[0].InnerXml,
                Type = xmlElement.GetElementsByTagName("Type")[0].Attributes[0].InnerXml,
                UpdatedTime = DateTime.Parse(xmlElement.GetElementsByTagName("UpdatedTime")[0].Attributes[0].InnerXml),
                UpdatedAuthor = xmlElement.GetElementsByTagName("UpdatedAuthor")[0].Attributes[0].InnerXml
            };
            return dBInformationModel;
        }
    }
}
