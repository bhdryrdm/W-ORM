using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using W_ORM.Layout.DBType;
using W_ORM.MSSQL;
using W_ORM.Test.MSSQL;
using W_ORM.Test.MSSQL.Entities;

namespace W_ORM.Test
{
    [TestClass]
    public class MSSQL_Unit_Tests
    {
        [TestMethod]
        public void CreateContextWormConfig()
        {
            WORM_Config_Operation.SaveWormConfig<University>("Server = .; Trusted_Connection = True;", DBType_Enum.MSSQL, "bhdryrdm");
        }

        [TestMethod]
        public void ContextGenerateFromDB()
        {
            WORM_Config_Operation.CreateContext<University>(12,"","");
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
        public void ToPaginateList()
        {
            University university = new University();
            List<Department> departmentList = university.Department.ToPaginateList(null,"DepartmentID", 2, 3);
        }

        [TestMethod]
        public void Where()
        {
            University university = new University();
            List<Student> studentList = university.Student.Where(x => x.DepartmentID == 1 && x.StudentEmail.Contains("Test") || x.StudentName != "Bahadır");
        }
    }
}
