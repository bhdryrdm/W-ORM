using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.Entities
{
    [Table(SchemaName ="dbo",TableName ="Student")]
    public class Student
    {
        [INT] [NOTNULL] public int StudentID { get; set; }
        [NVARCHAR(100)] [NOTNULL] public string StudentName { get; set; }
        [NVARCHAR(100)] [NOTNULL] public string StudentSurName { get; set; }
    }
}
