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
    
    
    [Table(TableName="TakeCourse", OrdinalPosition=4)]
    public sealed class TakeCourse
    {
        
        [FOREIGN_KEY("Student", "StudentID")]
        [INT()]
        [NOTNULL()]
        public int StudentID { get; set; } // StudentID;
        
        [FOREIGN_KEY("Course", "CourseID")]
        [INT()]
        [NOTNULL()]
        public int CourseID { get; set; } // CourseID;
        
        [VARCHAR(10)]
        [NOTNULL()]
        public string Grade { get; set; } // Grade;
    }
}
