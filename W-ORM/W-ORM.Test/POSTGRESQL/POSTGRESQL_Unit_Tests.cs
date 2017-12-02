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
    public class POSTGRESQL_Unit_Tests
    {
        [TestMethod]
        public void CreateEverythingForPOSTGRESQL()
        {
            CreateEverything<POSTGRESQL_University> createEverything = new CreateEverything<POSTGRESQL_University>();
            Tuple<string, string> tupleData = createEverything.EntityClassQueries();

            DB_Operation dB_Operation = new DB_Operation(typeof(POSTGRESQL_University).Name);
            dB_Operation.CreateORAlterDatabaseAndTables(tupleData.Item2, tupleData.Item1);
        }
    }
}
