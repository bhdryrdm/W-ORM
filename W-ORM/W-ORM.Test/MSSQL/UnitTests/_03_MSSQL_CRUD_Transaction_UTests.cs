using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using W_ORM.Test.MSSQL.Entities;

namespace W_ORM.Test.MSSQL.UnitTests
{
    [TestClass]
    public class _03_MSSQL_CRUD_Transaction_UTests
    {
        [TestMethod]
        public void InsertWithTransaction()
        {
            University university = new University();
            List<Department> departmentList = new List<Department>
            {
                new Department { DepartmentName = "Computer Engineering" },
                new Department { DepartmentName = "Mechanical Engineering" },
                new Department { DepartmentName = "Mathematics Engineering" },
            };
            for (int i = 0; i < 3000; i++)
            {
                departmentList.Add(new Department { DepartmentName = "Computer Engineering" + i });
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
            //University university = new University();

            //Department willBeUpdatingDepartment = new Department { DepartmentName = "Bahadır" };
            //Department willBeUpdatingDepartment1 = new Department { DepartmentName = "Yardım" };

            //var transaction = university.BeginTransaction();
            //university.Department.Update(x => x.DepartmentID == 5, willBeUpdatingDepartment, transaction);
            //university.Department.Update(x => x.DepartmentID == 6, willBeUpdatingDepartment1, transaction);
            //university.TransactionCommit(transaction);

            University university = new University();

            Department willBeUpdatingDepartment = new Department { DepartmentName = "UpdatingDepartmentName1" };
            Department willBeUpdatingDepartment1 = new Department { DepartmentName = "UpdatingDepartmentName2" };

            var transaction = university.BeginTransaction();
            university.Department.Update(x => x.DepartmentID == 18, willBeUpdatingDepartment, transaction);
            university.Department.Update(x => x.DepartmentID == 20, willBeUpdatingDepartment1, transaction);
            university.Department.TransactionCommit(transaction);

        }

        [TestMethod]
        public void DeleteWithTransaction()
        {
            //University university = new University();
            //var transaction = university.BeginTransaction();
            //university.Department.Delete(x => x.DepartmentID == 5, transaction);
            //university.Department.Delete(x => x.DepartmentID == 6, transaction);
            //university.Department.Delete(x => x.DepartmentID == 11, transaction);
            //university.Department.Delete(x => x.DepartmentID == 12, transaction);
            //university.Department.Delete(x => x.DepartmentID == 13, transaction);
            //university.Course.Delete(x => x.CourseID == 1, transaction);
            //university.TransactionCommit(transaction);

            University university = new University();
            var transaction = university.BeginTransaction();
            university.Department.Delete(x => x.DepartmentID == 5, transaction);
            university.Department.Delete(x => x.DepartmentID == 6, transaction);
            university.Department.Delete(x => x.DepartmentID == 11, transaction);
            university.Department.Delete(x => x.DepartmentID == 12, transaction);
            university.Department.Delete(x => x.DepartmentID == 13, transaction);
            university.TransactionCommit(transaction);

        }
    }
}
