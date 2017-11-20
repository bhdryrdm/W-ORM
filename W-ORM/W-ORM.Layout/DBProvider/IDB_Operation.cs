using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBProvider
{
    public interface  IDB_Operation
    {
        bool CreateDatabase();
        bool CreateSettingTable(string tablesXMLForm);
        bool CreateTable(string createTableSQLQuery);
    }
}
