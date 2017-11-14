using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Test
{
    [Schema(SchemaName = "dbo")]
    public class Product : ModelBase
    {
        [INT] public int ProductID { get; set; }
        [NVARCHAR] public string ProductName { get; set; }
        [BIT] public bool IsActive { get; set; }
        public decimal ProductPrice { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
