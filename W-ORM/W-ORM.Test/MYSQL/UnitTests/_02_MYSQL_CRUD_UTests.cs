using Microsoft.VisualStudio.TestTools.UnitTesting;
using W_ORM.MYSQL;
using W_ORM.MYSQL.WORM_Context.Entities;

namespace W_ORM.Test.MYSQL.UnitTests
{
    [TestClass]
    public class _02_MYSQL_CRUD_UTests
    {
        [TestMethod]
        public void Insert()
        {
            //MYSQL_University university = new MYSQL_University();
            //university.Department.Insert(new Department { DepartmentID = 1, DepartmentName = "Computer Engineering" });
            //university.PushToDB();

            MYSQL_University university = new MYSQL_University();
            university.Department.Insert(new Department { DepartmentID = 11, DepartmentName = "Biomedical Engineering" });
            university.PushToDB();

        }

        [TestMethod]
        public void Update()
        {
            //MYSQL_University university = new MYSQL_University();
            //Department willBeUpdatingDepartment = university.Department.FirstOrDefault(x => x.DepartmentID == 1);
            //willBeUpdatingDepartment.DepartmentName = "Computer Engineering Updated";
            //university.Department.Update(willBeUpdatingDepartment);
            //university.PushToDB();

            MYSQL_University university = new MYSQL_University();
            Department willBeUpdatingDepartment = university.Department.FirstOrDefault(x => x.DepartmentID == 2);
            willBeUpdatingDepartment.DepartmentName = "UpdatingDepartment";
            university.Department.Update(willBeUpdatingDepartment);
            university.PushToDB();
        }

        [TestMethod]
        public void Delete()
        {
            //MYSQL_University university = new MYSQL_University();
            //Department willBeDeletedDepartment = university.Department.FirstOrDefault(x => x.DepartmentID == 1);
            //university.Department.Delete(willBeDeletedDepartment);
            //university.PushToDB();

            MYSQL_University university = new MYSQL_University();
            Department willBeDeletedDepartment = university.Department.FirstOrDefault(x => x.DepartmentID == 2);
            university.Department.Delete(willBeDeletedDepartment);
            university.PushToDB();
        }
    }
}
