﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.Entities
{
    [Table(SchemaName = "dbo", TableName = "Student", OrdinalPosition = 2)]
    public class Student
    {
        [FOREIGN_KEY("Department", "DepartmentID")] [INT] public int DepartmentID { get; set; }
        [PRIMARY_KEY] [INT] [NOTNULL] public int StudentID { get; set; }
        [NVARCHAR(100)] [NOTNULL] public string StudentName { get; set; }
        [NVARCHAR(100)] [NOTNULL] public string StudentSurName { get; set; }
        [NVARCHAR(100)] [NOTNULL] public string StudentEmail { get; set; }
    }
}