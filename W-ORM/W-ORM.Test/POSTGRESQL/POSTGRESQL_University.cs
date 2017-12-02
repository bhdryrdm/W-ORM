using W_ORM.POSTGRESQL;
using W_ORM.Test.POSTGRESQL.Entities;

namespace W_ORM.Test.POSTGRESQL
{
    public class POSTGRESQL_University
    {
        public POSTGRESQLProviderContext<Student> Student { get { return new POSTGRESQLProviderContext<Student>(); } }
        public POSTGRESQLProviderContext<Department> Department { get { return new POSTGRESQLProviderContext<Department>(); } }
    }
}
