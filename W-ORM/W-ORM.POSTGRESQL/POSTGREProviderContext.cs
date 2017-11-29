using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBProvider;

namespace W_ORM.POSTGRESQL
{
    public class POSTGREProviderContext<TDBEntity> : IDB_CRUD_Operation<TDBEntity>
    {
        public void Delete(TDBEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(TDBEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TDBEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
