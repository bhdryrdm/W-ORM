using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBProvider;

namespace W_ORM.MYSQL
{
    public class MYSQLProviderContext<TDBEntity> : IDB_CRUD_Operation<TDBEntity>
    {
        protected Type EntityType { get { return typeof(TDBEntity); } }

        public void Insert(TDBEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TDBEntity entity)
        {
            throw new NotImplementedException();
        }
        public void Delete(TDBEntity entity)
        {
            throw new NotImplementedException();
        }

        public int PushToDB()
        {
            throw new NotImplementedException();
        }
    }
}
