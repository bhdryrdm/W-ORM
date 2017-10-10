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
    public class LessonsConfiguration : EntityTypeConfiguration<Lessons>
    {
        public LessonsConfiguration()
        {
            // Table Name
            this.ToTable("Lessons");

            // Primary Key
            this.HasKey<int>(x => x.LessonID);

            #region Property Defination

            //Column property
            this.Property(x => x.LessonID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("LessonID")
                .HasColumnOrder(1)
                .HasColumnType("int")
                .IsRequired();

            this.Property(x => x.LessonName)
                .HasColumnName("LessonName")
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
