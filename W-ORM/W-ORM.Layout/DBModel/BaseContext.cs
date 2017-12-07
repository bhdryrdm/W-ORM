using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBConnection;

namespace W_ORM.Layout.DBModel
{
    public class BaseContext
    { 
        DbConnection connection = null;
        DbCommand command = null;

        public static string runQuery = string.Empty;
        public static Dictionary<string, object> parameterList = new Dictionary<string, object>();
        public int PushToDB(string contextName)
        {
            try
            {
                using (connection = DBConnectionFactory.Instance(contextName))
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
                return 0;
                throw ex;
            }
        }
    }
}
