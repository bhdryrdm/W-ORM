using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using W_ORM.Test.MSSQL.Entities;

namespace W_ORM.Test.MSSQL.UnitTests
{
    [TestClass]
    public class _04_MSSQL_CRUD_Helper_UTests
    {
        [TestMethod]
        public void FirstOrDefault()
        {
            University university = new University();
            university.Student.FirstOrDefault(x => x.DepartmentID == 1);
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
            List<Department> departmentList = university.Department.ToPaginateList(null, "DepartmentID", 2, 3);
        }

        [TestMethod]
        public void Where()
        {
            University university = new University();
            List<Student> studentList = university.Student.Where(x => x.DepartmentID == 1 && x.StudentEmail.Contains("Test") || x.StudentName != "Bahadır");
        }
    }
}
