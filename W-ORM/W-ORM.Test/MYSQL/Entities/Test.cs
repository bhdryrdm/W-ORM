//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace W_ORM.MYSQL.WORM_Context.Entities
{
    using W_ORM.Layout.Attributes;
    using W_ORM.MYSQL.Attributes;
    
    
    [Table(TableName="Test", OrdinalPosition=1)]
    public sealed class Test
    {
        
        [PRIMARY_KEY()]
        [INT()]
        [NOTNULL()]
        public int TestID { get; set; } // TestID;
        
        [VARCHAR(100)]
        [NOTNULL()]
        public string TestName { get; set; } // TestName;
    }
}
