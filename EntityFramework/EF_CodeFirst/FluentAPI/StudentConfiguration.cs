using EF_CodeFirst.EntityTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst.FluentAPI
{
    public class StudentConfiguration : EntityTypeConfiguration<Student>
    {
        public StudentConfiguration()
        {
            // Table Name
            this.ToTable("Student");

            // Primary Key
            this.HasKey<int>(x => x.StudentID);

            #region Property Defination

            //Column property
            this.Property(x => x.StudentID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("StudentID")
                .HasColumnOrder(1)
                .HasColumnType("int")
                .IsRequired();

            this.Property(x => x.StudentName)
                .HasColumnName("StudentName")
                .HasMaxLength(500)
                .HasColumnOrder(2)
                .HasColumnType("nvarchar")
                .IsRequired();

            this.Property(x => x.StudentSurname)
               .HasColumnName("StudentSurname")
               .HasMaxLength(500)
               .HasColumnOrder(3)
               .HasColumnType("nvarchar")
               .IsRequired();

            this.Property(x => x.CreatedTime)
                .HasColumnName("CreatedTime")
                .HasColumnOrder(4)
                .HasColumnType("datetime")
                .IsRequired();

            this.Property(x => x.UpdatedTime)
                .HasColumnName("UpdatedTime")
                .HasColumnOrder(5)
                .HasColumnType("datetime")
                .IsRequired();
            #endregion
        }
    }
}
