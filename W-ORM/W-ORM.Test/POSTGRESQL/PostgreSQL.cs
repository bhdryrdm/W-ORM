using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.POSTGRESQL;

namespace W_ORM.Test.POSTGRESQL
{
    [TestClass]
    public class PostgreSQL
    {
        [TestMethod]
        public void CreateDatabase()
        {
            DB_Operation dB_Operation = new DB_Operation("POSTGRESQL-BHDR");
            dB_Operation.CreateDatabase();
        }


    }
}
