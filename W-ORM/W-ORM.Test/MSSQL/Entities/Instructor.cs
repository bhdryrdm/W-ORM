using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.Entities
{
    [Table(SchemaName = "dbo", TableName = "Instructor", OrdinalPosition = 5)]
    public class Instructor
    {
        [PRIMARY_KEY][AUTO_INCREMENT(1, 10)][INT][NOTNULL]public int InstructorID { get; set; }
        [NVARCHAR(100)][NOTNULL]public string InstructorName { get; set; }
        [NVARCHAR(100)][NOTNULL]public string InstructorSurName { get; set; }
        [FOREIGN_KEY("Department", "DepartmentID")][INT][NOTNULL]public int DepartmentID { get; set; }
    }
}

