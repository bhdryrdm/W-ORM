using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst.EntityTables
{
    public interface IAudit
    {
        DateTime CreatedTime { get; set; }
        DateTime UpdatedTime { get; set; }
    }
}
