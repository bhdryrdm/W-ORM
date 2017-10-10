using EF_CodeFirst.EntityTables;
using EF_CodeFirst.FluentAPI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst.ContextConfiguration
{
    public class UniversityContext : DbContext
    {
        public UniversityContext() : base("name=UniversityContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UniversityContext, Configuration>("UniversityContext"));
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Lecturer> Lecturer { get; set; }
        public DbSet<Lessons> Lessons { get; set; }
        public DbSet<Student> Student { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using Fluent API here
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new LecturerConfiguration());
            modelBuilder.Configurations.Add(new LessonsConfiguration());
            modelBuilder.Configurations.Add(new StudentConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
