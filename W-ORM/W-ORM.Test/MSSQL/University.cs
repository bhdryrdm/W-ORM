using W_ORM.MSSQL;
using W_ORM.Test.MSSQL.Entities;

namespace W_ORM.Test.MSSQL
{
    public class University 
    {
        public MSSQLProviderContext<Student> Student { get { return new MSSQLProviderContext<Student>(); } }
        public MSSQLProviderContext<Department> Department { get { return new MSSQLProviderContext<Department>(); } }
    }
}
