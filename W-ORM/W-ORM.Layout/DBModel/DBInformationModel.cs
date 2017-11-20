using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBModel
{
    public class DBInformationModel
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string Provider { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedAuthor { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string UpdatedAuthor { get; set; }
    }
}
