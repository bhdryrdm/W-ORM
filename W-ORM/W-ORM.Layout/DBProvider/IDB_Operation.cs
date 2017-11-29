using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBProvider
{
    public interface IDB_Operation
    {
        string ContextName { get; set; }
        bool CreateDatabase();
        bool CreateSettingTable(string tablesXMLForm);
        bool CreateTable(string createTableSQLQuery);
        bool ContextGenerateFromDB(int dbVersion,string contextPath="",string contextName="");
    }
}
