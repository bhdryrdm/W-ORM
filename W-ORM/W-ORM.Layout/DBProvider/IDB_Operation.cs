using System;
using System.Data.SqlClient;

namespace W_ORM.Layout.DBProvider
{
    public interface IDB_Operation
    {
        string ContextName { get; set; }
        bool CreateORAlterDatabaseAndTables(string tablesXMLForm, string createTableSQLQuery);
    }
}
