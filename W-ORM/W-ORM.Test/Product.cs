using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test
{
    [Table(TableName = "Product", SchemaName = "dbo")]
    public class Product : ModelBase
    {
        [INT] public int ProductID { get; set; }
        [NVARCHAR] public string ProductName { get; set; }
        [BIT] public bool IsActive { get; set; }
    }
}
