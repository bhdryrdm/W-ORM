using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test
{

    [Table(SchemaName = "dbo",TableName ="Category")]
    public class Category : ModelBase
    {
        [INT] public int CategoryID { get; set; }
        [NVARCHAR] public string ProductName { get; set; }
        [BIT] public bool IsActive { get; set; }
    }
}
