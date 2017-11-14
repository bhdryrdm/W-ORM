using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBType;

namespace W_ORM.Layout.DBProvider
{
    public interface IDBProvider
    {
        string ConnectionString { get; set; }
        DBType_Enum Type { get; set; }
        string Author { get; set; }
    }
}
