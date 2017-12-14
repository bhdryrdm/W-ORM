using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.Entities
{
    [Table(SchemaName = "dbo", TableName = "Student", OrdinalPosition = 2)]
    public class Student
    {
        [PRIMARY_KEY][AUTO_INCREMENT(1,1)][INT] [NOTNULL] public int StudentID { get; set; }
        [NVARCHAR(100)] [NOTNULL] public string StudentName { get; set; }
        [NVARCHAR(100)] [NOTNULL] public string StudentSurName { get; set; }
        [NVARCHAR(100)] [NOTNULL] public string StudentEmail { get; set; }
        [FOREIGN_KEY("Department", "DepartmentID")] [INT] [NOTNULL] public int DepartmentID { get; set; }
    }
}
