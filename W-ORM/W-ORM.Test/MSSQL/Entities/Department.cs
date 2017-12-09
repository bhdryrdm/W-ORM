using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.Entities
{
    [Table(SchemaName ="dbo", TableName ="Department", OrdinalPosition = 1)]
    public class Department
    {
        [PRIMARY_KEY][INT][NOTNULL] public int DepartmentID { get; set; }
        [NVARCHAR(500)] [NOTNULL] public string DepartmentName { get; set; }
    }
}
