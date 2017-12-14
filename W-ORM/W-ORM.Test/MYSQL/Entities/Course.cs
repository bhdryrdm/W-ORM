using W_ORM.Layout.Attributes;
using W_ORM.MYSQL.Attributes;

namespace W_ORM.Test.MYSQL.Entities
{
    [Table(TableName = "Course", OrdinalPosition = 3)]
    public class Course
    {
        [PRIMARY_KEY][AUTO_INCREMENT][INT][NOTNULL]public int CourseID { get; set; }
        [VARCHAR(500)][NOTNULL]public string CourseName { get; set; }
        [VARCHAR(10)][NOTNULL]public string CourseCredit { get; set; }
        [FOREIGN_KEY("Department", "DepartmentID")][INT][NOTNULL]public int DepartmentID { get; set; }
    }
}

