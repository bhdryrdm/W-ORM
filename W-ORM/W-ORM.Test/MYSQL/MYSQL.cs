using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using W_ORM.MYSQL;

namespace W_ORM.Test.MYSQL
{
    /// <summary>
    /// Summary description for MYSQL
    /// </summary>
    [TestClass]
    public class MYSQL
    {
        [TestMethod]
        public void CreateDatabase()
        {
            DB_Operation dB_Operation = new DB_Operation("MYSQL-BHDR");
            dB_Operation.CreateDatabase();
        }
        
      
    }
}
