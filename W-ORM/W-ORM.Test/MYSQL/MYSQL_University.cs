using W_ORM.Layout.DBModel;
using W_ORM.MYSQL;
using W_ORM.Test.MYSQL.Entities;

namespace W_ORM.Test.MYSQL
{
    public class MYSQL_University : BaseContext<MYSQL_University>
    {
        public MYSQLProviderContext<Student> Student { get { return new MYSQLProviderContext<Student>(); } }
        public MYSQLProviderContext<Department> Department { get { return new MYSQLProviderContext<Department>(); } }
        public MYSQLProviderContext<Course> Course { get { return new MYSQLProviderContext<Course>(); } }
        public MYSQLProviderContext<TakeCourse> TakeCourse { get { return new MYSQLProviderContext<TakeCourse>(); } }

    }
}
