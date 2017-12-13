using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBType
{
    public static class DBType_To_DBProvider
    {

        public static string GetProviderFactoryByEnum(DBType_Enum dBType_Enum)
        {
            string returnValue = string.Empty;
            switch (dBType_Enum)
            {
                case DBType_Enum.MSSQL:
                    returnValue = "System.Data.SqlClient"; break;
                case DBType_Enum.MYSQL:
                    returnValue = "MySql.Data.MySqlClient"; break;
                default: break;
            }
            return returnValue;
        }
    }
}
