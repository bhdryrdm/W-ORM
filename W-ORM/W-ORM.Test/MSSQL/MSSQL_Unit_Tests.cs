using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using W_ORM.Layout.DBType;
using W_ORM.MSSQL;
using W_ORM.Test.MSSQL.Entities;

namespace W_ORM.Test.MSSQL
{
    [TestClass]
    public class MSSQL_Unit_Tests
    {
        public static string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        [TestMethod]
        public void CreateContextWormConfig()
        {
            WORM_Config_Creator.SaveWormConfig<University>(path, "Server = .; Trusted_Connection = True;", DBType_Enum.MSSQL, "bhdryrdm");
        }

        [TestMethod]
        public void Insert()
        {
            University university = new University();
            university.Department.Insert(new Department { DepartmentID = 1 });
            university.PushToDB();
        }

        public void ToList()
        {
            University university = new University();
            List<Department> departmentList = university.Department.ToList(); 
        }

        [TestMethod]
        public void ContextGenerateFromDB()
        {
            DB_Operation dB_Operation = new DB_Operation(typeof(University).Name);
            dB_Operation.ContextGenerateFromDB(1, "", "", "BHDR_Context");
        }

        [TestMethod]
        public void CreateEverythingForMSSQL()
        {
            CreateEverything<University> createEverything = new CreateEverything<University>();
            Tuple<string, string> tupleData = createEverything.EntityClassQueries();

            DB_Operation dB_Operation = new DB_Operation(typeof(University).Name);
            dB_Operation.CreateORAlterDatabaseAndTables(tupleData.Item2, tupleData.Item1);
        }
    }
}
