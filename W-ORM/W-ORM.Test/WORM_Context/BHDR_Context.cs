//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace W_ORM.Test.WORM_Context
{
    using W_ORM.Layout.DBModel;
    using W_ORM.MSSQL;
    using W_ORM.Test.WORM_Context.Entities;
    
    
    public class BHDR_Context : BaseContext<BHDR_Context>
    {
        
        public MSSQLProviderContext<Department,BHDR_Context> Department
        {
            get
            {
                return new MSSQLProviderContext<Department,BHDR_Context>();
            }
        }
        
        public MSSQLProviderContext<Student,BHDR_Context> Student
        {
            get
            {
                return new MSSQLProviderContext<Student,BHDR_Context>();
            }
        }
    }
}
