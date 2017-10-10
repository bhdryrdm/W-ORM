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
    public class LecturerConfiguration : EntityTypeConfiguration<Lecturer>
    {
        public LecturerConfiguration()
        {
            // Table Name
            this.ToTable("Lecturer");

            // Primary Key
            this.HasKey<int>(x => x.LecturerID);

            #region Property Defination

            //Column property
            this.Property(x => x.LecturerID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("LecturerID")
                .HasColumnOrder(1)
                .HasColumnType("int")
                .IsRequired();

            this.Property(x => x.LecturerName)
                .HasColumnName("LecturerName")
                .HasMaxLength(500)
                .HasColumnOrder(2)
                .HasColumnType("nvarchar")
                .IsRequired();

            this.Property(x => x.LecturerSurname)
               .HasColumnName("LecturerSurname")
               .HasMaxLength(500)
               .HasColumnOrder(3)
               .HasColumnType("nvarchar")
               .IsRequired();

            this.Property(x => x.DepartmentID)
               .HasColumnName("DepartmentID")
               .HasColumnOrder(4)
               .HasColumnType("int")
               .IsRequired();

            this.Property(x => x.CreatedTime)
                .HasColumnName("CreatedTime")
                .HasColumnOrder(5)
                .HasColumnType("datetime")
                .IsRequired();

            this.Property(x => x.UpdatedTime)
                .HasColumnName("UpdatedTime")
                .HasColumnOrder(6)
                .HasColumnType("datetime")
                .IsRequired();
            #endregion
        }
    }
}
