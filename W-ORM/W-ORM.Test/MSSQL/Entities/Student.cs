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
        [PRIMARY_KEY] [INT] public int StudentID { get; set; }
        [NVARCHAR(100)] public string StudentName { get; set; }
    }
}
