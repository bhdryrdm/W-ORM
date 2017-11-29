using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test
{
    [Table(SchemaName = "dbo", TableName = "Product")]
    public class Product
    {
        [FOREIGN_KEY("Category","CategoryID")] [INT] public int CategoryID {get; set; }
    }
}
