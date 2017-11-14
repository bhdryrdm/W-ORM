using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace W_ORM.MSSQL
{
    public class WORMContext : WORM_MSSQL_Configuration
    {
        public WORMContext() : base("Server=.;Database=TestORM; Trusted_Connection=True;",Layout.DBType.DBType_Enum.MSSQL,"Bahadır Yardım")
        {

        }

    }
}
