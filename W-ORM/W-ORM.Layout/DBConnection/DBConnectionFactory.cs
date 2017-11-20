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

        private DBConnectionFactory()
        {

        }

        #region I.Veritabanı yada genel olarak kullanım için gerekli olan kod bloğu
        public static DbConnection Instance(string contextID, string databaseName = "")
        {
            if (dbConnectionString == null)
            {
                lock (objectLockControl)
                {
                    if (dbConnectionString == null)
                    {
                        var dbInformation = ReturnDBInformatinFromXML(contextID);
                        DbProviderFactory factory = DbProviderFactories.GetFactory(dbInformation.Provider);
                        DbConnection connection = factory.CreateConnection();
                        connection.ConnectionString = $"{dbInformation.ConnectionString}";
                        if (!String.IsNullOrEmpty(databaseName))
                        {
                            connection.ConnectionString += $"; Database={databaseName};";
                        }
                        return connection;
                    }
                }
            }
            return dbConnectionString;
        }
        #endregion

        public static DBInformationModel ReturnDBInformatinFromXML(string contextID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\WORM.config");
            XmlElement xmlElement = (XmlElement)xmlDoc.DocumentElement.SelectSingleNode($"{contextID}[@id='{contextID}']");
            DBInformationModel dBInformationModel = new DBInformationModel
            {
                DatabaseName = xmlElement.GetElementsByTagName("DatabaseName")[0].Attributes[0].InnerXml,
                ConnectionString = xmlElement.GetElementsByTagName("ConnectionString")[0].Attributes[0].InnerXml,
                Provider = xmlElement.GetElementsByTagName("Provider")[0].Attributes[0].InnerXml,
                Type = xmlElement.GetElementsByTagName("Type")[0].Attributes[0].InnerXml,
                Version = int.Parse(xmlElement.GetElementsByTagName("Version")[0].Attributes[0].InnerXml),
                CreatedTime = DateTime.Parse(xmlElement.GetElementsByTagName("CreatedTime")[0].Attributes[0].InnerXml),
                CreatedAuthor = xmlElement.GetElementsByTagName("CreatedAuthor")[0].Attributes[0].InnerXml,
                UpdatedTime = DateTime.Parse(xmlElement.GetElementsByTagName("UpdatedTime")[0].Attributes[0].InnerXml),
                UpdatedAuthor = xmlElement.GetElementsByTagName("UpdatedAuthor")[0].Attributes[0].InnerXml
            };
            return dBInformationModel;
        }
    }
}
