using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.BankEntities
{
    [Table(SchemaName = "dbo", TableName = "Loan")]
    public class Loan
    {
        [INT][NOTNULL]public int LoanNumber { get; set; }
        [INT][NOTNULL]public int Amount { get; set; }
    }
}
