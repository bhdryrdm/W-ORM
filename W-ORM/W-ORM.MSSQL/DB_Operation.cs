using System;
using System.Data;
using System.Data.Common;
using W_ORM.Layout.DBConnection;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;

namespace W_ORM.MSSQL
{
    public class DB_Operation :  IDB_Operation 
    {
         DbCommand command = null;
        DbConnection connection = null;

        #region Property & Constructor
        private string databaseName;

        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        private string contextName;

        public string ContextName
        {
            get { return contextName; }
            set { contextName = value; }
        }

        public DB_Operation(string contextName, string databaseName)
        {
            this.contextName = contextName;
            this.databaseName = databaseName;
        }
        #endregion

        public bool CreateDatabase()
        {
            bool dbCreatedSuccess = true;
            try
            {
                using (connection = DBConnectionFactory.Instance(this.ContextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    command = connection.CreateCommand();
                    command.CommandText = $"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = N'{this.databaseName}')" +
                                          "BEGIN " +
                                          $"CREATE DATABASE {this.databaseName} " +
                                          "END";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                dbCreatedSuccess = false;
            }
            DBConnectionOperation.ConnectionClose(connection);
            return dbCreatedSuccess;
        }

        public bool CreateSettingTable(string tablesXMLForm)
        {
            bool dbTabledSuccess = true;
            try
            {
                using (connection = DBConnectionFactory.Instance(this.ContextName, this.databaseName))
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
                                          "TablesXMLForm nvarchar(max)" +
                                          ") END";
                    command.ExecuteNonQuery();

                    command = null;
                    command = connection.CreateCommand();
                    DBInformationModel dBInformationModel = DBConnectionFactory.ReturnDBInformatinFromXML(this.ContextName);
                    command.CommandText = $"INSERT INTO __WORM_Configuration " +
                        $"(DatabaseName, Version, CreatedTime, CreatedAuthor, UpdatedTime, UpdatedAuthor, TablesXMLForm) " +
                        $@"VALUES({dBInformationModel.DatabaseName}, {dBInformationModel.Version}, {dBInformationModel.CreatedTime}, " +
                        $@"{dBInformationModel.CreatedAuthor},{dBInformationModel.UpdatedTime},{dBInformationModel.UpdatedAuthor},N'{tablesXMLForm})";
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

        public bool CreateTable(string createTableSQLQuery)
        {
            bool dbTabledSuccess = true;
            try
            {
                using (connection = DBConnectionFactory.Instance(ContextName, databaseName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    command = connection.CreateCommand();
                    command.CommandText = createTableSQLQuery;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                dbTabledSuccess = false;
            }
            DBConnectionOperation.ConnectionClose(connection);
            return dbTabledSuccess;
        }
    }
}
