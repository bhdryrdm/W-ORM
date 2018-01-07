using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using W_ORM.Layout.DBType;
using W_ORM.MSSQL;

namespace W_ORM.Test.MSSQL.UnitTests
{
    [TestClass]
    public class _01_MSSQL_Database_Operation_UTests
    {
        [TestMethod]
        public void CreateContextWormConfig() => WORM_Config_Operation.SaveWormConfig<University>("Server = aws-mssql.ck1c2q7swb3a.us-west-2.rds.amazonaws.com;User Id = bhdryrdm; Password = bhdryrdm54;", DBType_Enum.MSSQL, "rcpypc");

        [TestMethod]
        public void ContextGenerateFromDB() => WORM_Config_Operation.CreateContext<University>(1, "", "");

        [TestMethod]
        public void CreateOrAlterDatabase()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            CreateDatabase<University> createDatabase = new CreateDatabase<University>();
            Tuple<string, string> tupleData = createDatabase.EntityClassQueries();
            DB_Operation dbOperation = new DB_Operation(typeof(University).Name);
            dbOperation.CreateOrAlterTable(tupleData.Item1);
            stopwatch.Stop();
            string a = $"{stopwatch.Elapsed.Hours}:{stopwatch.Elapsed.Minutes}:{stopwatch.Elapsed.Seconds}:{stopwatch.Elapsed.Milliseconds}";
        }


    }
}
