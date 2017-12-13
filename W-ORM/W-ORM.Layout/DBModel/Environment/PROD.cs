using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBConnection;

namespace W_ORM.Layout.DBModel.Environment
{
    public class PROD<TContextName>
    {
        DbCommand command = null;
        DbConnection connection = null;
        public static string runQuery;
        public static Dictionary<string, object> parameterList = new Dictionary<string, object>();

        /// <summary>
        /// TR : CRUD işlemlerinden sonra çalıştırılacak method
        /// EN :
        /// </summary>
        /// <returns></returns>
        protected int SendToDB()
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
    }
}
