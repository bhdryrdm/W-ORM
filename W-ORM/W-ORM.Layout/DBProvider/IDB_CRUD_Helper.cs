using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBProvider
{
    /// <summary>
    /// TR : CRUD işlemleri için gerekli olabilecek yardımcı methodlar 
    /// EN : 
    /// </summary>
    /// <typeparam name="TEntity">Entity Class(POCO)</typeparam>
    public interface IDB_CRUD_Helper<TEntity>
    {
        List<TEntity> ToList();
        List<TEntity> ToPaginateList(Expression<Func<TEntity, object>> predicate, string orderByColumn, int pageSize, int requestedPageNumber);
        TEntity FirstOrDefault(Expression<Func<TEntity, object>> predicate);
        List<TEntity> Where(Expression<Func<TEntity, object>> predicate);
    }
}
