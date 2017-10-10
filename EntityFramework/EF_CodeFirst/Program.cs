using EF_CodeFirst.ContextConfiguration;
using EF_CodeFirst.EntityTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (UniversityContext dbContext = new UniversityContext())
                {
                    Department department = new Department
                    {
                        DepartmentName = "Software Engineering",
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now
                    };

                    dbContext.Department.Add(department);
                    dbContext.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }

        }
    }
}
