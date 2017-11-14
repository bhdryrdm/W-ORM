using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Test
{
    public class WORMContext
    {
        public WORMContext()
        {

        }
        public MSSQLProviderContext<Product> Product { get; set; }
        public MSSQLProviderContext<Category> Category { get; set; }
    }
}
