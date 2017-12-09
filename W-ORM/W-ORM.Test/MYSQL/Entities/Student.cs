using W_ORM.Layout.Attributes;
using W_ORM.MYSQL.Attributes;

namespace W_ORM.Test.MYSQL.Entities
{
    [Table(TableName = "Student", OrdinalPosition = 2)]
    public class Student
    {
        [PRIMARY_KEY][INT] [NOTNULL] public int StudentID { get; set; }
        [VARCHAR(100)] [NOTNULL] public string StudentName { get; set; }
        [VARCHAR(100)] [NOTNULL] public string StudentSurName { get; set; }
        [VARCHAR(100)] [NOTNULL] public string StudentEmail { get; set; }
        [FOREIGN_KEY("Department", "DepartmentID")][INT]public int DepartmentID { get; set; }
    }
}
