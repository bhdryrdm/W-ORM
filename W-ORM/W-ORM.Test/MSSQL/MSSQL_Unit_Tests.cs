using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
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
        public void CreateEverythingForMSSQL()
        {
            CreateEverything<University> createEverything = new CreateEverything<University>();
            Tuple<string, string> tupleData = createEverything.EntityClassQueries();

            DB_Operation dB_Operation = new DB_Operation(typeof(University).Name);
            dB_Operation.CreateORAlterDatabaseAndTables(tupleData.Item2, tupleData.Item1);
        }

        [TestMethod]
        public void ContextGenerateFromDB()
        {
            DB_Operation dB_Operation = new DB_Operation(typeof(University).Name);
            dB_Operation.ContextGenerateFromDB(1, "", "", "BHDR_Context");
        }
    }

    [TestClass]
    public class MSSQL_CRUD_Unit_Tests
    {
        [TestMethod]
        public void Insert()
        {
            University university = new University();
            university.Department.Insert(new Department { DepartmentName = "Computer Engineering" });
            university.PushToDB();
        }

        [TestMethod]
        public void Update()
        {
            University university = new University();
            Department willBeUpdatingDepartment= university.Department.FirstOrDefault(x => x.DepartmentID == 1);
            willBeUpdatingDepartment.DepartmentName = "UpdatingDepartment";
            university.Department.Update(willBeUpdatingDepartment);
            university.PushToDB();
        }

        [TestMethod]
        public void Delete()
        {
            University university = new University();
            Department willBeDeletedDepartment = university.Department.FirstOrDefault(x => x.DepartmentID == 1);
            university.Department.Delete(willBeDeletedDepartment);
            university.PushToDB();
        }
    }

    [TestClass]
    public class MSSQL_CRUD_Helper_Unit_Tests
    {
        [TestMethod]
        public void FirstOrDefault()
        {
            University university = new University();
            university.Student.FirstOrDefault(x => x.DepartmentID == 1 && x.StudentEmail.Contains("Test") || x.StudentName != "Bahadır");
        }

        [TestMethod]
        public void ToList()
        {
            University university = new University();
            List<Department> departmentList = university.Department.ToList();
        }

        [TestMethod]
        public void Where()
        {
            University university = new University();
            List<Student> studentList = university.Student.Where(x => x.DepartmentID == 1 && x.StudentEmail.Contains("Test") || x.StudentName != "Bahadır");
        }
    }
}
