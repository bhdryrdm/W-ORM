using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using W_ORM.MYSQL;
using W_ORM.MYSQL.WORM_Context.Entities;

namespace W_ORM.Test.MYSQL.UnitTests
{
    [TestClass]
    public class _03_MYSQL_CRUD_Transaction_UTests
    {
        [TestMethod]
        public void InsertWithTransaction()
        {
            MYSQL_University university = new MYSQL_University();
            List<Department> departmentList = new List<Department>
            {
                new Department {DepartmentID = 2, DepartmentName = "Computer Engineering" },
                new Department {DepartmentID = 3, DepartmentName = "Mechanical Engineering" },
                new Department {DepartmentID = 4, DepartmentName = "Mathematics Engineering" },
            };
            for (int i = 0; i < 3000; i++)
            {
                departmentList.Add(new Department { DepartmentID= i+5, DepartmentName = "Computer Engineering" + i });
            }

            var transaction = university.BeginTransaction();
            foreach (var department in departmentList)
            {
                university.Department.Insert(department, transaction);
            }
            university.Department.TransactionCommit(transaction);
        }

        [TestMethod]
        public void UpdateWithTransaction()
        {
            MYSQL_University university = new MYSQL_University();

            Department willBeUpdatingDepartment = new Department { DepartmentID = 12345, DepartmentName = "Bahadır" };
            Department willBeUpdatingDepartment1 = new Department { DepartmentID = 12346, DepartmentName = "Yardım" };

            var transaction = university.BeginTransaction();
            university.Department.Update(x => x.DepartmentID == 5, willBeUpdatingDepartment, transaction);
            university.Department.Update(x => x.DepartmentID == 6, willBeUpdatingDepartment1, transaction);
            university.TransactionCommit(transaction);
        }

        [TestMethod]
        public void DeleteWithTransaction()
        {
            MYSQL_University university = new MYSQL_University();
            var transaction = university.BeginTransaction();
            university.Department.Delete(x => x.DepartmentID == 12345, transaction);
            university.Department.Delete(x => x.DepartmentID == 12346, transaction);
            university.Department.Delete(x => x.DepartmentID == 11, transaction);
            university.Department.Delete(x => x.DepartmentID == 12, transaction);
            university.Department.Delete(x => x.DepartmentID == 13, transaction);
            university.Course.Delete(x => x.CourseID == 1, transaction);
            university.TransactionCommit(transaction);

        }
    }
}
