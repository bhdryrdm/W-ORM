using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBConnection
{
    public static class DBConnectionOperation
    {
        public static DbConnection ConnectionOpen(DbConnection connection)
        {
            DbConnection returnConnection = null;
            if(connection.State != ConnectionState.Open)
            {
                connection.Open();
                returnConnection = connection;
            }
            return returnConnection;
        }
        public static DbConnection ConnectionClose(DbConnection connection)
        {
            DbConnection returnConnection = null;
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
                returnConnection = connection;
            }
            return returnConnection;
        }
    }
}
