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
    
    
    [Table(TableName="Student", OrdinalPosition=2)]
    public sealed class Student
    {
        
        [PRIMARY_KEY()]
        [INT()]
        [NOTNULL()]
        public int StudentID { get; set; } // StudentID;
        
        [VARCHAR(100)]
        [NOTNULL()]
        public string StudentName { get; set; } // StudentName;
        
        [VARCHAR(100)]
        [NOTNULL()]
        public string StudentSurName { get; set; } // StudentSurName;
        
        [VARCHAR(100)]
        [NOTNULL()]
        public string StudentEmail { get; set; } // StudentEmail;
        
        [FOREIGN_KEY("Department", "DepartmentID")]
        [INT()]
        public int DepartmentID { get; set; } // DepartmentID;
    }
}
