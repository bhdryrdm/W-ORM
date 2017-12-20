using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using W_ORM.Layout.DBType;
using W_ORM.MYSQL;
using W_ORM.MYSQL.WORM_Context;
using W_ORM.MYSQL.WORM_Context.Entities;

namespace W_ORM.Test
{
    [TestClass]
    public class MYSQL_Unit_Tests
    {
        [TestMethod]
        public void CreateContextWormConfig()
        {
            WORM_Config_Operation.SaveWormConfig<MYSQL_University>("Server = localhost; Port = 3306; Uid=root; Pwd = 123qwe;", DBType_Enum.MYSQL, "bhdryrdm");
        }

        [TestMethod]
        public void ContextGenerateFromDB()
        {
            WORM_Config_Operation.CreateContext<MYSQL_University>(21, "F:\\01-GITLAB\\10-W-ORM\\W-ORM\\W-ORM.Test\\MYSQL\\", "");
        }
    }

    [TestClass]
    public class MYSQL_CRUD_Unit_Tests
    {
        [TestMethod]
        public void Insert()
        {
            MYSQL_University university = new MYSQL_University();
            university.Department.Insert(new Department { DepartmentID=1, DepartmentName = "Computer Engineering" });
            university.PushToDB();
        }

        [TestMethod]
        public void Update()
        {
            MYSQL_University university = new MYSQL_University();
            Department willBeUpdatingDepartment = university.Department.FirstOrDefault(x => x.DepartmentID == 1);
            willBeUpdatingDepartment.DepartmentName = "Computer Engineering Updated";
            university.Department.Update(willBeUpdatingDepartment);
            university.PushToDB();
        }

        [TestMethod]
        public void Delete()
        {
            MYSQL_University university = new MYSQL_University();
            Department willBeDeletedDepartment = university.Department.FirstOrDefault(x => x.DepartmentID == 1);
            university.Department.Delete(willBeDeletedDepartment);
            university.PushToDB();
        }
    }

    [TestClass]
    public class MYSQL_CRUD_Helper_Unit_Tests
    {
        [TestMethod]
        public void FirstOrDefault()
        {
            MYSQL_University university = new MYSQL_University();
            university.Student.FirstOrDefault(x => x.DepartmentID == 1 && x.StudentEmail.Contains("Test") || x.StudentName != "Bahadır");
        }

        [TestMethod]
        public void ToList()
        {
            MYSQL_University university = new MYSQL_University();
            List<Department> departmentList = university.Department.ToList();
        }

        [TestMethod]
        public void ToPaginateList()
        {
            MYSQL_University university = new MYSQL_University();
           List<Department> departmentList = university.Department.ToPaginateList(null, "DepartmentID", 2, 3);
        }

        [TestMethod]
        public void Where()
        {
            MYSQL_University university = new MYSQL_University();
            List<Student> studentList = university.Student.Where(x => x.DepartmentID == 1 && x.StudentEmail.Contains("Test") || x.StudentName != "Bahadır");
        }
    }
}
