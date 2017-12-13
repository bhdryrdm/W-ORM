using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MYSQL.Entities
{
    [Table(TableName = "TakeCourse", OrdinalPosition = 4)]
    public class TakeCourse
    {
        [FOREIGN_KEY("Student", "StudentID")][INT][NOTNULL]public int StudentID { get; set; }
        [FOREIGN_KEY("Course", "CourseID")][INT][NOTNULL]public int CourseID { get; set; }
        [NVARCHAR(10)][NOTNULL]public string Grade { get; set; }
    }
}
