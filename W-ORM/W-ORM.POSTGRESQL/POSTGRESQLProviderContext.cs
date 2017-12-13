using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;

namespace W_ORM.POSTGRESQL
{
    public class POSTGRESQLProviderContext<TDBEntity,TContextName> : BaseContext<TContextName>,IDB_CRUD_Operation<TDBEntity>
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

        public TDBEntity FirstOrDefault()
        {
            throw new NotImplementedException();
        }

        public TDBEntity FirstOrDefault(Expression<Func<TDBEntity, object>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Insert(TDBEntity entity)
        {
            throw new NotImplementedException();
        }

        public List<TDBEntity> ToList()
        {
            throw new NotImplementedException();
        }

        public List<TDBEntity> ToPaginateList(Expression<Func<TDBEntity, object>> predicate, string orderByColumn, int pageSize, int requestedPageNumber)
        {
            throw new NotImplementedException();
        }

        public void Update(TDBEntity entity)
        {
            throw new NotImplementedException();
        }

        public List<TDBEntity> Where(Expression<Func<TDBEntity, object>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
