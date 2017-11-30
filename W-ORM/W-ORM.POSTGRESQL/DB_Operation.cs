using System;
using System.Data.Common;
using W_ORM.Layout.DBProvider;

namespace W_ORM.POSTGRESQL
{
    public class DB_Operation : IDB_Operation
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
            throw new NotImplementedException();
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
