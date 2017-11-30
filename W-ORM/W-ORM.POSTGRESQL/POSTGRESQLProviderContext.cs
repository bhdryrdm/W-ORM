using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBConnection;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;

namespace W_ORM.POSTGRESQL
{
    public class POSTGRESQLProviderContext<TDBEntity> : BaseContext, IDB_CRUD_Operation<TDBEntity>
    {
        protected string EntitySchema
        {
            get
            {
                dynamic entitySchema = EntityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
                return entitySchema.SchemaName ? entitySchema.SchemaName : "dbo";
            }
        }

        protected Type EntityType
        {
            get
            {
                return typeof(TDBEntity);
            }
        }

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
