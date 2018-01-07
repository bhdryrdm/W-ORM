using Microsoft.VisualStudio.TestTools.UnitTesting;
using W_ORM.Layout.DBType;
using W_ORM.MYSQL;

namespace W_ORM.Test.MYSQL.UnitTests
{
    [TestClass]
    public class _01_MYSQL_Database_Operation_UTests
    {
        [TestMethod]
        public void CreateContextWormConfig() => WORM_Config_Operation.SaveWormConfig<MYSQL_University>("Server = 130.211.138.151; Port = 3306; Uid=root; Pwd = bhdryrdm54;", DBType_Enum.MYSQL, "bhdryrdm");

        [TestMethod]
        public void ContextGenerateFromDB() => WORM_Config_Operation.CreateContext<MYSQL_University>(21, "F:\\01-GITLAB\\10-W-ORM\\W-ORM\\W-ORM.Test\\MYSQL\\", "");
    }
}
