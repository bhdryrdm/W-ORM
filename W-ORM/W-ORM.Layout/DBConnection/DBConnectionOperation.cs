using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBConnection
{
    /// <summary>
    /// TR : Bağlantı kontrolü
    /// EN : Connection Control
    /// </summary>
    public static class DBConnectionOperation
    {
        /// <summary>
        /// TR : Connection Is Open ? Control 
        /// </summary>
        /// <param name="connection"> TR : Bağlantı Nesnesi EN : Connection Object</param>
        /// <returns></returns>
        public static DbConnection ConnectionOpen(DbConnection connection)
        {
            DbConnection returnConnection = null;
            if(connection != null && connection.State != ConnectionState.Open)
            {
                connection.Open();
                returnConnection = connection;
            }
            return returnConnection;
        }

        /// <summary>
        /// TR : Connection Is Close ? Control 
        /// </summary>
        /// <param name="connection"> TR : Bağlantı Nesnesi EN : Connection Object</param>
        /// <returns></returns>
        public static DbConnection ConnectionClose(DbConnection connection)
        {
            DbConnection returnConnection = null;
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
                returnConnection = connection;
            }
            return returnConnection;
        }
    }
}
