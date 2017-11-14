using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBProvider;
using W_ORM.Layout.DBType;

namespace W_ORM.MSSQL
{
    public class WORM_MSSQL_Configuration : IDBProvider
    {
        #region IDBProvider
        public string ConnectionString { get; set; }
        public DBType_Enum Type { get; set; }
        public string Author { get; set; }
        #endregion


        public WORM_MSSQL_Configuration(string connectionString,DBType_Enum type,string author)
        {
            this.ConnectionString = connectionString;
            this.Type = type;
            this.Author = author;
        }
        
    }
}
