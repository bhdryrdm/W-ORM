using System;
using System.Data.SqlClient;

namespace W_ORM.Layout.DBProvider
{
    public interface IDB_Operation
    {
        string ContextName { get; set; }
        bool CreateORAlterDatabaseAndTables(string tablesXMLForm, string createTableSQLQuery);
        string CreateDatabaseQuery();
        Tuple<string, SqlCommand> Create__WORM__Configuration_Table(string tablesXMLForm);
    }
}
