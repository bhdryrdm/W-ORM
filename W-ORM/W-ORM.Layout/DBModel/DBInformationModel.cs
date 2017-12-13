using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBModel
{
    /// <summary>
    /// TR : WORM.config dosyasından okunan bilgileri tutmak için kullanılır
    /// EN : 
    /// </summary>
    public class DBInformationModel
    {
        public int Version { get; set; }
        public string ConnectionString { get; set; }
        public string Provider { get; set; }
        public string UpdatedAuthor { get; set; }
    }
}
