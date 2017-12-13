using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBProvider
{
    /// <summary>
    /// TR : CRUD işlemleri
    /// EN : CRUD Operations
    /// </summary>
    /// <typeparam name="TEntity">Entity Class(POCO)</typeparam>
    public interface IDB_CRUD_Operation<TEntity> : IDB_CRUD_Helper<TEntity>
    {
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
