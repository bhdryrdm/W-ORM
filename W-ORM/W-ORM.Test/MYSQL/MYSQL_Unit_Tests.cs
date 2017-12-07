using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using W_ORM.MYSQL;

namespace W_ORM.Test.MYSQL
{
    [TestClass]
    public class MYSQL_Unit_Tests
    {
        [TestMethod]
        public void CreateEverythingForMYSQL()
        {
            CreateEverything<MYSQL_University> createEverything = new CreateEverything<MYSQL_University>();
            Tuple<string, string> tupleData = createEverything.EntityClassQueries();

            DB_Operation dB_Operation = new DB_Operation(typeof(MYSQL_University).Name);
            dB_Operation.CreateORAlterDatabaseAndTables(tupleData.Item2, tupleData.Item1);

            
        }
    }
}
