using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.Entities
{
    [Table(SchemaName = "dbo", TableName = "Teach", OrdinalPosition = 6)]
    public class Teach
    {
        [FOREIGN_KEY("Course", "CourseID")][INT][NOTNULL]public int CourseID { get; set; }
        [FOREIGN_KEY("Instructor", "InstructorID")][INT][NOTNULL]public int InstructorID { get; set; }
    }
}