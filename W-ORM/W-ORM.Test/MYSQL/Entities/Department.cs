using W_ORM.Layout.Attributes;
using W_ORM.MYSQL.Attributes;

namespace W_ORM.Test.MYSQL.Entities
{
    [Table(TableName = "Department", OrdinalPosition = 1)]
    public class Department
    {
        [PRIMARY_KEY] [AUTO_INCREMENT] [INT] [NOTNULL] public int DepartmentID { get; set; }
        [VARCHAR(100)] [NOTNULL] public int DepartmentName { get; set; }
    }
}
