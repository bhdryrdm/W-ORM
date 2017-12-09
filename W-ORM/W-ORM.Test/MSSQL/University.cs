using W_ORM.Layout.DBModel;
using W_ORM.MSSQL;
using W_ORM.Test.MSSQL.Entities;

namespace W_ORM.Test.MSSQL
{
    public class University : BaseContext<University>
    {
        public MSSQLProviderContext<Student,University> Student { get { return new MSSQLProviderContext<Student,University>(); } }
        public MSSQLProviderContext<Department,University> Department { get { return new MSSQLProviderContext<Department,University>(); } }
    }
}
