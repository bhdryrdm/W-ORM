using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.MYSQL.Attributes;

namespace W_ORM.Test.MYSQL.Entities
{
    [Table(SchemaName = "dbo", TableName = "Category")]
    public class Student
    {
        [INT] public int StudentID { get; set; }
        [VARCHAR(300)] public string  StudentName { get; set; }
    }
}
