using W_ORM.Layout.DBModel;
using W_ORM.MSSQL;

namespace W_ORM.Test
{
    public class RECEPContext : BaseContext
    {
        public MSSQLProviderContext<Category> Category
        {
            get { return new MSSQLProviderContext<Category>(); }
        }
        public MSSQLProviderContext<Product> Product
        {
            get { return new MSSQLProviderContext<Product>(); }
        }
    }
}
