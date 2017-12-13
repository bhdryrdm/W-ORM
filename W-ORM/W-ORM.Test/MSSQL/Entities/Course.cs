
using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.Entities
{
    [Table(SchemaName = "dbo", TableName = "Course", OrdinalPosition = 3)]
    public class Course
    {
        [PRIMARY_KEY] [AUTO_INCREMENT(1, 1)] [INT] [NOTNULL] public int CourseID { get; set; }
        [NVARCHAR(500)] [NOTNULL] public string CourseName { get; set; }
        [NVARCHAR(10)] [NOTNULL] public string CourseCredit { get; set; }
        [FOREIGN_KEY("Department","DepartmentID")] [INT] [NOTNULL] public int DepartmentID { get; set; }
    }
}