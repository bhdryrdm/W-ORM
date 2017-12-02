using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.MYSQL;
using W_ORM.Test.MYSQL.Entities;

namespace W_ORM.Test.MYSQL
{
    public class MYSQL_University
    {
        public MYSQLProviderContext<Student> Student { get { return new MYSQLProviderContext<Student>(); } }
        public MYSQLProviderContext<Department> Department { get { return new MYSQLProviderContext<Department>(); } }
    }
}
