using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using W_ORM.Layout.DBConnection;
using W_ORM.Layout.DBProvider;

namespace W_ORM.MYSQL
{
    public class DB_Operation : IDB_Operation
    {
        DbCommand command = null;
        DbConnection connection = null;
        
        #region Property & Constructor
      
        private string contextName;

        public string ContextName
        {
            get { return contextName; }
            set { contextName = value; }
        }

        public DB_Operation(string contextName)
        {
            this.contextName = contextName;
        }
        #endregion

        public bool CreateDatabase(string tablesXMLForm)
        {
            bool dbCreatedSuccess = true;
            try
            {
                using (connection = DBConnectionFactory.CreateDatabaseInstance(this.contextName))
                {
                    
                    DBConnectionOperation.ConnectionOpen(connection);
                    MySqlCommand command = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{this.contextName.ToLower()}`",(MySqlConnection)connection);
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
            throw new NotImplementedException();
        }

        public bool CreateTable(string createTableSQLQuery)
        {
            throw new NotImplementedException();
        }

        public bool ContextGenerateFromDB(int dbVersion, string contextPath = "", string contextName = "")
        {
            throw new NotImplementedException();
        }

        public bool CreateORAlterDatabaseAndTables(string tablesXMLForm, string createTableSQLQuery)
        {
            throw new NotImplementedException();
        }
    }
}
