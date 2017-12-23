namespace W_ORM.MYSQL
{
    using W_ORM.Layout.DBModel;
    using W_ORM.MYSQL.WORM_Context.Entities;
    
    
    public class MYSQL_University : BaseContext<MYSQL_University>
    {
        public MYSQLProviderContext<Student, MYSQL_University> Student { get { return new MYSQLProviderContext<Student, MYSQL_University>(); } }
        public MYSQLProviderContext<Department, MYSQL_University> Department { get { return new MYSQLProviderContext<Department, MYSQL_University>(); } }
        public MYSQLProviderContext<Course, MYSQL_University> Course { get { return new MYSQLProviderContext<Course, MYSQL_University>(); } }
        public MYSQLProviderContext<TakeCourse, MYSQL_University> TakeCourse { get { return new MYSQLProviderContext<TakeCourse, MYSQL_University>(); } }
    }
}
