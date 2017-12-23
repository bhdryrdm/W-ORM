using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using W_ORM.MYSQL;
using W_ORM.MYSQL.WORM_Context.Entities;

namespace W_ORM.Test.MYSQL.UnitTests
{
    [TestClass]
    public class _04_MYSQL_CRUD_Helper_UTests
    {
        [TestMethod]
        public void FirstOrDefault()
        {
            MYSQL_University university = new MYSQL_University();
            university.Student.FirstOrDefault(x => x.DepartmentID == 1);
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
