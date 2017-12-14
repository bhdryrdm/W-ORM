using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.Entities
{
    [Table(SchemaName ="dbo", TableName ="Department", OrdinalPosition = 1)]
    public class Department
    {
        [PRIMARY_KEY][AUTO_INCREMENT(1,1)][INT][NOTNULL] public int DepartmentID { get; set; }
        [NVARCHAR(100)] [NOTNULL] public string DepartmentName { get; set; }
    }
}
