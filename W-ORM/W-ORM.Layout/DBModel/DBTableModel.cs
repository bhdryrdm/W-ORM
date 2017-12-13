using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBModel
{
    /// <summary>
    /// TR : Entity Class (POCO) için tanımlanmış olan Table Attribute çözümlenerek bilgilerin tutulması için kullanılır
    /// EN : 
    /// </summary>
    public class DBTableModel
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
    }
}
