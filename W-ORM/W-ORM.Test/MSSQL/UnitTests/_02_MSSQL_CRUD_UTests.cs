using Microsoft.VisualStudio.TestTools.UnitTesting;
using W_ORM.Test.MSSQL.Entities;

namespace W_ORM.Test.MSSQL.UnitTests
{
    [TestClass]
    public class _02_MSSQL_CRUD_UTests
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
            Department willBeUpdatingDepartment = university.Department.FirstOrDefault(x => x.DepartmentID == 1);
            willBeUpdatingDepartment.DepartmentName = "UpdatingDepartment";
            university.Department.Update(willBeUpdatingDepartment);
            university.PushToDB();
        }

        [TestMethod]
        public void Delete()
        {
            University university = new University();
            Department willBeDeletedDepartment = university.Department.FirstOrDefault(x => x.DepartmentID == 2);
            university.Department.Delete(willBeDeletedDepartment);
            university.PushToDB();
        }
    }
}
