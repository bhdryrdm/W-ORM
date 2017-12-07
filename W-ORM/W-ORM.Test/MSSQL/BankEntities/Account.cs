using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.BankEntities
{
    [Table(SchemaName = "dbo", TableName = "Account")]
    public class Account
    {
        [INT][NOTNULL]public int AccountID { get; set; }
        [INT][NOTNULL]public int AccountBalance { get; set; }
        [NVARCHAR(500)][NOTNULL]public string AccountCategory{ get; set; }
        [NVARCHAR(500)][NOTNULL]public string AccountName { get; set; }
        [INT][NOTNULL]public int AccountCode { get; set; }
        [NVARCHAR(500)][NOTNULL]public string AccountType { get; set; }
    }
}
