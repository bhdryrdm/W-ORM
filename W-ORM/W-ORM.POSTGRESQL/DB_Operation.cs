using System;
using System.Data.Common;
using System.Data.OleDb;
using W_ORM.Layout.DBConnection;
using W_ORM.Layout.DBProvider;

namespace W_ORM.POSTGRESQL
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

        public bool ContextGenerateFromDB(int dbVersion, string contextPath = "", string contextName = "")

        {
            throw new NotImplementedException();
        }

        public bool CreateDatabase(string tablesXMLForm)
        {
            bool dbCreatedSuccess = true;
            try
            {
                using (connection = DBConnectionFactory.Instance("POSTGRESQL-BHDR"))
                {

                    DBConnectionOperation.ConnectionOpen(connection);
                    OleDbCommand command = new OleDbCommand("BEGIN " +
                                                              "IF EXISTS(SELECT 1 FROM pg_database WHERE datname = 'mydb') THEN " +
                                                              "ELSE " +
                                                              "PERFORM dblink_exec('dbname=' || current_database()-- current db " +
                                                              $", 'CREATE DATABASE {this.contextName}');" +
                                                              "END IF; " +
                                                              "END", (OleDbConnection)connection);
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
    }

}