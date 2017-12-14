using W_ORM.Layout.DBModel;
using W_ORM.MSSQL;
using W_ORM.Test.MSSQL.Entities;

namespace W_ORM.Test.MSSQL
{
    public class University : BaseContext<University>
    {
        public MSSQLProviderContext<Student,University> Student { get { return new MSSQLProviderContext<Student,University>(); } }
        public MSSQLProviderContext<Department,University> Department { get { return new MSSQLProviderContext<Department,University>(); } }
        public MSSQLProviderContext<Course, University> Course { get { return new MSSQLProviderContext<Course, University>(); } }
        public MSSQLProviderContext<TakeCourse, University> TakeCourse { get { return new MSSQLProviderContext<TakeCourse, University>(); } }
        public MSSQLProviderContext<Instructor, University> Instructor { get { return new MSSQLProviderContext<Instructor, University>(); } }
        public MSSQLProviderContext<Teach, University> Teach { get { return new MSSQLProviderContext<Teach, University>(); } }
    }
}
