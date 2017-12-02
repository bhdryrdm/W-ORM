using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.POSTGRESQL.Attributes;

namespace W_ORM.Test.POSTGRESQL.Entities
{
    [Table(TableName = "Department")]
    public class Department
    {
        [INT] [NOTNULL] public int DepartmentID { get; set; }
        [VARCHAR(500)] [NOTNULL] public string DepartmentName { get; set; }
    }
}
