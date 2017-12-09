using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBConnection;

namespace W_ORM.Layout.DBModel
{
    public class BaseContext<TContextName>
    {
        DbConnection connection = null;
        DbCommand command = null;

        public static string runQuery;

        public static Dictionary<string, object> parameterList = new Dictionary<string, object>();
        public int PushToDB()
        {
            try
            {
                using (connection = DBConnectionFactory.Instance(typeof(TContextName).Name))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    command = connection.CreateCommand();
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

        protected List<TEntity> GetListFromDB<TEntity>()
        {
            List<TEntity> entities = new List<TEntity>();
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
