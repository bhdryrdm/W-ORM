using Microsoft.VisualStudio.TestTools.UnitTesting;
using W_ORM.Layout.DBType;
using W_ORM.MSSQL;

namespace W_ORM.Test.MSSQL.UnitTests
{
    [TestClass]
    public class _01_MSSQL_Database_Operation_UTests
    {
        [TestMethod]
        public void CreateContextWormConfig() => WORM_Config_Operation.SaveWormConfig<University>("Server = .; Trusted_Connection = True;", DBType_Enum.MSSQL, "bhdryrdm");

        [TestMethod]
        public void ContextGenerateFromDB() => WORM_Config_Operation.CreateContext<University>(1, "", "");
    }
}
