﻿using W_ORM.Layout.DBModel;
using W_ORM.MSSQL;

namespace W_ORM.Test
{
    public class WORMContext : BaseContext
    {
        public MSSQLProviderContext<Category> Category
        {
            get { return new MSSQLProviderContext<Category>(); }
        }
    }
}
