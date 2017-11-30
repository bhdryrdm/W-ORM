using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test
{
    
    [Table(SchemaName = "dbo",TableName ="Category")]
    public class Category
    {
        [AUTO_INCREMENT(1,5)][PRIMARY_KEY][INT] public int CategoryID { get; set; }
        [NVARCHAR(500)] public string ProductName { get; set; }
        [BIT] public bool IsActive { get; set; }
    }
}
