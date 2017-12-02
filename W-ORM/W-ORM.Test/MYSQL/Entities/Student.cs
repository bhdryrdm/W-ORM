using W_ORM.Layout.Attributes;
using W_ORM.MYSQL.Attributes;

namespace W_ORM.Test.MYSQL.Entities
{
    [Table(TableName = "Student")]
    public class Student
    {
        [INT] [NOTNULL] public int StudentID { get; set; }
        [VARCHAR(100)] [NOTNULL] public string StudentName { get; set; }
        [VARCHAR(100)] [NOTNULL] public string StudentSurName { get; set; }
        [VARCHAR(100)] [NOTNULL] public string StudentEmail { get; set; }
    }
}
