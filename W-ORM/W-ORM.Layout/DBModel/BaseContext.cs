using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using W_ORM.Layout.DBConnection;

namespace W_ORM.Layout.DBModel
{
    /// <summary>
    /// TR : Oluşturulan Context Adı
    /// EN : 
    /// </summary>
    /// <typeparam name="TContextName"></typeparam>
    public class BaseContext<TContextName>
    {
        protected DbCommand command = null;
        protected DbConnection connection = null;
        protected static string runQuery;
        protected static string runVersionQuery;
        protected static Dictionary<string, object> parameterList = new Dictionary<string, object>();
        private static string contextName = typeof(TContextName).Name;

        /// <summary>
        /// TR : CRUD işlemlerinden sonra çalıştırılacak method
        /// EN :
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public virtual int PushToDB()
        {
            try
            {
                using (connection = DBConnectionFactory.Instance(contextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    command = connection.CreateCommand();
                    command.CommandText = runVersionQuery;

                    //Development ortamı isteyen geliştiriciler override edip bu satırları aktif hale getirecekler 
                    //if (DBConnectionFactory.LatestDatabaseVersionFromXML(contextName) != Convert.ToInt32(command.ExecuteScalar()))
                    //  throw new Exception("Veritabanı üzerinde bir güncelleme olduğu için bu işlem gerçekleştirilemez.Lütfen öncelikle Veritabanı versiyonunuzu eşitleyiniz.");

                    command.CommandText = runQuery;
                    if (parameterList != null && parameterList.Count > 0)
                    {
                        foreach (var loopParameter in parameterList)
                        {
                            DbParameter parameter = command.CreateParameter();
                            parameter.ParameterName = loopParameter.Key.ToString();
                            parameter.Value = loopParameter.Value;
                            command.Parameters.Add(parameter);
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                throw ex;
            }
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
                using (connection = DBConnectionFactory.Instance(contextName))
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
                using (connection = DBConnectionFactory.Instance(contextName))
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

        public DbTransaction BeginTransaction()
        {
            try
            {
                connection = DBConnectionFactory.Instance(contextName);
                DBConnectionOperation.ConnectionOpen(connection);
                return connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                throw ex;
            }
        }

        protected int BaseTransaction(DbTransaction transaction)
        {
            try
            {
                connection = DBConnectionFactory.Instance(contextName);
                DBConnectionOperation.ConnectionOpen(connection);
                command = connection.CreateCommand();
                command.Transaction = transaction;

                command.CommandText = runQuery;
                if (parameterList != null && parameterList.Count > 0)
                {
                    foreach (var loopParameter in parameterList)
                    {
                        DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = loopParameter.Key.ToString();
                        parameter.Value = loopParameter.Value;
                        command.Parameters.Add(parameter);
                    }
                }
                return command.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                throw ex;
            }
        }

        public void TransactionCommit(DbTransaction transaction)
        {
            try
            {
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                DBConnectionOperation.ConnectionClose(connection);
                throw new Exception("Transaction çalıştırılırken hata oluştu!.RollBack çalıştırıldı.",ex);
            }
        }
    }
}
