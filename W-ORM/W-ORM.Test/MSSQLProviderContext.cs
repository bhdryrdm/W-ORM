using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Test
{
    public abstract class MSSQLProviderContext<TDBEntity> : DatabasesProviderBase 
    {
        protected Type EntityType{ get { return typeof(TDBEntity);}}
    }
}
