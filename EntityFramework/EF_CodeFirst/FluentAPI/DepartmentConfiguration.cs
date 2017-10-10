using EF_CodeFirst.EntityTables;
using System.Data.Entity.ModelConfiguration;

namespace EF_CodeFirst.FluentAPI
{
    public class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            // Table Name
            this.ToTable("Department");

            // Primary Key
            this.HasKey<int>(x => x.DepartmentID);

            #region Property Defination
            
            //Column property
            this.Property(x => x.DepartmentID)
                .HasColumnName("DepartmentID")
                .HasColumnOrder(1)
                .HasColumnType("int")
                .IsRequired();

            this.Property(x => x.DepartmentName)
                .HasColumnName("DepartmentName")
                .HasMaxLength(500)
                .HasColumnOrder(2)
                .HasColumnType("nvarchar")
                .IsRequired();

            this.Property(x => x.CreatedTime)
                .HasColumnName("CreatedTime")
                .HasColumnOrder(3)
                .HasColumnType("datetime")
                .IsRequired();

            this.Property(x => x.UpdatedTime)
                .HasColumnName("UpdatedTime")
                .HasColumnOrder(4)
                .HasColumnType("datetime")
                .IsRequired();
            #endregion
        }
    }
}
