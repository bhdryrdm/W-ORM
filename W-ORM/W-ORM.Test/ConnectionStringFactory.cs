using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml;

namespace W_ORM.Test
{
    public class ConnectionStringFactory
    {

        private static DbConnection dbConnectionString;
        private static Object objectLockControl = new Object();

        private ConnectionStringFactory()
        {
            
        }

        #region I.Veritabanı yada genel olarak kullanım için gerekli olan kod bloğu
        //Burada direkt (SqlConnection) döndüm çünkü bana bu gerekli sadece (String) ConnectingString yeterli olmayabilir 
        public static DbConnection Instance(string contextID, string databaseName="")
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
                            connection.ConnectionString += $" Database={databaseName};";
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
        public static bool CreateDB(string databaseName)
        {
            DbCommand command = null;
            DbConnection connection = null;
            bool dbCreatedSuccess = true;
            try
            {
                using (connection = Instance("WORMContext"))
                {
                    connection.Open();
                    command = connection.CreateCommand(); 
                    command.CommandText = $"IF  NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'{databaseName}')" +
                                           "BEGIN " +
                                          $"CREATE DATABASE {databaseName} " +
                                           "END";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                
                dbCreatedSuccess = false;
            }

            if (connection.State == ConnectionState.Open || connection.State == ConnectionState.Connecting)
            {
                connection.Close();
            }
            return dbCreatedSuccess;
        }
        public static bool CreateSettingTable(string databaseName,string JSONObject)
        {
            DbCommand command = null;
            DbConnection connection = null;
            bool dbTabledSuccess = true;
            try
            {
                using (connection = Instance("WORMContext", databaseName))
                {
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = "IF  NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'__WORM_Configuration')" +
                                          "BEGIN " +
                                          "CREATE TABLE [dbo].[__WORM_Configuration]( " +
                                          "DatabaseName nvarchar," +
                                          "Version int," +
                                          "CreatedTime datetime," +
                                          "CreatedAuthor nvarchar," +
                                          "UpdatedTime datetime," +
                                          "UpdatedAuthor nvarchar," +
                                          "JSONTable nvarchar(max)" +
                                          ") END";
                    command.ExecuteNonQuery();

                    command = null;
                    command = connection.CreateCommand();
                    DBInformationModel dBInformationModel = ReturnDBInformatinFromXML("WORMContext");
                    command.CommandText = $"INSERT INTO __WORM_Configuration " +
                        $"(DatabaseName, Version, CreatedTime, CreatedAuthor, UpdatedTime, UpdatedAuthor, JSONTable) " +
                        $@"VALUES({dBInformationModel.DatabaseName}, {dBInformationModel.Version}, {dBInformationModel.CreatedTime}, " +
                        $@"{dBInformationModel.CreatedAuthor},{dBInformationModel.UpdatedTime},{dBInformationModel.UpdatedAuthor},{JSONObject})";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                dbTabledSuccess = false;
            }

            if (connection.State == ConnectionState.Open || connection.State == ConnectionState.Connecting)
            {
                connection.Close();
            }
            return dbTabledSuccess;
        }
        public static bool CreateTable(string databaseName,string createTableSQLQuery)
        {
            DbCommand command = null;
            DbConnection connection = null;
            bool dbTabledSuccess = true;
            try
            {
                using (connection = Instance("WORMContext", databaseName))
                {
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = createTableSQLQuery;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                dbTabledSuccess = false;
            }

            if (connection.State == ConnectionState.Open || connection.State == ConnectionState.Connecting)
            {
                connection.Close();
            }
            return dbTabledSuccess;
        }

    }
}

