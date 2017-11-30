<<<<<<< HEAD
﻿using System;
using System.Data.Common;
=======
﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBConnection;
>>>>>>> f39a5d4865cbce928b573d39dbf3bd842d2306be
using W_ORM.Layout.DBProvider;

namespace W_ORM.POSTGRESQL
{
    public class DB_Operation : IDB_Operation
    {
        DbCommand command = null;
        DbConnection connection = null;

        #region Property & Constructor
<<<<<<< HEAD
        private string databaseName;

        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }
=======
>>>>>>> f39a5d4865cbce928b573d39dbf3bd842d2306be

        private string contextName;

        public string ContextName
        {
            get { return contextName; }
            set { contextName = value; }
        }
<<<<<<< HEAD
        public DB_Operation(string contextName, string databaseName)
        {
            this.contextName = contextName;
            this.databaseName = databaseName;
        }
        #endregion
        public bool CreateDatabase()
=======

        public DB_Operation(string contextName)
        {
            this.contextName = contextName;
        }
        #endregion
        

        public bool ContextGenerateFromDB(int dbVersion, string contextPath = "", string contextName = "")
>>>>>>> f39a5d4865cbce928b573d39dbf3bd842d2306be
        {
            throw new NotImplementedException();
        }

<<<<<<< HEAD
=======
        public bool CreateDatabase()
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
                                                              "END IF; "+
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

>>>>>>> f39a5d4865cbce928b573d39dbf3bd842d2306be
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
