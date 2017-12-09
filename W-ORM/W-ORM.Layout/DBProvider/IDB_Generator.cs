using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBProvider
{
    public interface IDB_Generator
    {
        bool ContextGenerateFromDB(int dbVersion, string contextPath = "", string namespaceName = "", string contextName = "");
    }
}
