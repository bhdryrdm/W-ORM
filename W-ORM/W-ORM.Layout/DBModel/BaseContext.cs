using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using W_ORM.Layout.DBConnection;
using W_ORM.Layout.DBModel.Environment;

namespace W_ORM.Layout.DBModel
{
    /// <summary>
    /// TR : Oluşturulan Context Adı
    /// EN : 
    /// </summary>
    /// <typeparam name="TContextName"></typeparam>
    public class BaseContext<TContextName> : DEV<TContextName>
    {
        DbCommand command = null; 
        DbConnection connection = null;

        /// <summary>
        /// TR : CRUD işlemlerinden sonra çalıştırılacak method
        /// EN :
        /// </summary>
        /// <returns></returns>
        public int PushToDB()
        {
            return base.SendToDB();
        }

        /// <summary>
        /// TR : Verilen Entity Class(POCO) yani Tabloya ait tüm kayıtları listelemek için kullanılır
        /// EN : 
        /// </summary>
        /// <typeparam name="TEntity">TR : Entity Class(POCO)/Tablo EN : Entity Class(POCO)/Table </typeparam>
        /// <returns></returns>
        protected List<TEntity> GetListFromDB<TEntity>()
        {
            List<TEntity> entities = new List<TEntity>();
            try
            {
                using (connection = DBConnectionFactory.Instance(typeof(TContextName).Name))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    command = connection.CreateCommand();
                    command.CommandText = runQuery;

                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        TEntity entity = Activator.CreateInstance<TEntity>();
                        foreach (PropertyInfo property in typeof(TEntity).GetProperties())
                        {
                            var propertyValue = reader[property.Name];
                            if (propertyValue == DBNull.Value)
                                propertyValue = null;

                            property.SetValue(entity, propertyValue);
                        }
                        entities.Add(entity);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                throw ex;
            }
            return entities;
        }

        /// <summary>
        /// TR : Verilen Entity Class(POCO) yani Tabloya ait tek bir kaydı getirmek için kullanılır
        /// EN : 
        /// </summary>
        /// <typeparam name="TEntity">TR : Entity Class(POCO)/Tablo EN : Entity Class(POCO)/Table</typeparam>
        /// <returns></returns>
        protected TEntity GetItemFromDB<TEntity>()
        {
            TEntity entity = Activator.CreateInstance<TEntity>();
            try
            {
                using (connection = DBConnectionFactory.Instance(typeof(TContextName).Name))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    command = connection.CreateCommand();
                    command.CommandText = runQuery;

                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        foreach (PropertyInfo property in typeof(TEntity).GetProperties())
                        {
                            var propertyValue = reader[property.Name];
                            if (propertyValue == DBNull.Value)
                                propertyValue = null;

                            property.SetValue(entity, propertyValue);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                throw ex;
            }
            return entity;
        }
    }
}
