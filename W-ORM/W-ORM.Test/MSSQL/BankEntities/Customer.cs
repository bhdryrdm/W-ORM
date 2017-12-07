using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.Test.MSSQL.BankEntities
{
    [Table(SchemaName = "dbo", TableName = "Customer")]
    public class Customer
    {
        [INT][NOTNULL]public int CustomerID { get; set; }
        [NVARCHAR(500)][NOTNULL]public string CustomerName { get; set; }
        [NVARCHAR(500)][NOTNULL]public string CustomerCity{ get; set; }
        [INT][NOTNULL]public int CustomerCode { get; set; }
        [NVARCHAR(500)][NOTNULL]public string CustomerMidddlename { get; set; }
        [NVARCHAR(500)][NOTNULL]public string CustomerType { get; set; }
        [NVARCHAR(500)][NOTNULL]public string CustomerSurname { get; set; }
    }
}
