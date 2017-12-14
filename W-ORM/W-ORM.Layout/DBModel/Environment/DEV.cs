using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBConnection;

namespace W_ORM.Layout.DBModel.Environment
{
    public class DEV<TContextName>
    {
        DbCommand command = null;
        DbConnection connection = null;
        protected static string runQuery;
        protected static string runVersionQuery;
        protected static Dictionary<string, object> parameterList = new Dictionary<string, object>();

        /// <summary>
        /// TR : CRUD işlemlerinden sonra çalıştırılacak method
        /// EN :
        /// </summary>
        /// <returns></returns>
        protected int SendToDB()
        {
            string contextName = typeof(TContextName).Name;
            try
            {
                using (connection = DBConnectionFactory.Instance(contextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    command = connection.CreateCommand();
                    command.CommandText = runVersionQuery;

                    if (DBConnectionFactory.LatestDatabaseVersionFromXML(contextName) != Convert.ToInt32(command.ExecuteScalar()))
                        throw new Exception("Veritabanı üzerinde bir güncelleme olduğu için bu işlem gerçekleştirilemez.Lütfen öncelikle Veritabanı versiyonunuzu eşitleyiniz.");

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
    }
}
