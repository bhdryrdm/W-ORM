using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBProvider
{
    public interface IDB_CRUD_Helper<TEntity>
    {
        List<TEntity> ToList();
        TEntity FirstOrDefault(Expression<Func<TEntity, object>> predicate);
        List<TEntity> Where(Expression<Func<TEntity, object>> predicate);
    }
}
