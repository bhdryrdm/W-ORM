using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBProvider;

namespace W_ORM.Layout.Query
{
    public interface IEntityClassQueryGenerator<TDBEntity> where TDBEntity : class
    {
        Tuple<string, string> EntityClassQueries(); 
    }
}
