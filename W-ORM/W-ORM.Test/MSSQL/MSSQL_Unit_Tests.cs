using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.MSSQL;

namespace W_ORM.Test.MSSQL
{
    [TestClass]
    public class MSSQL_Unit_Tests
    {
        [TestMethod]
        public void CreateEverythingForMSSQL()
        {
            CreateEverything<University> createEverything = new CreateEverything<University>();
            Tuple<string,string> tupleData = createEverything.EntityClassQueries();

            DB_Operation dB_Operation = new DB_Operation(typeof(University).Name);
            dB_Operation.CreateORAlterDatabaseAndTables(tupleData.Item2,tupleData.Item1);

        }
    }
}
