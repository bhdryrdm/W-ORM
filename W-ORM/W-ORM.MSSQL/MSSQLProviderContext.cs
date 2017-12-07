using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBConnection;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.MSSQL
{
    public class MSSQLProviderContext<TDBEntity> : BaseContext, IDB_CRUD_Operation<TDBEntity>
    {
        #region Entity Type & Entity Schema
        protected string EntitySchema
        {
            get
            {
                dynamic entitySchema = EntityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
                return String.IsNullOrEmpty(entitySchema.SchemaName) ? entitySchema.SchemaName : "dbo";
            }
        }

        protected Type EntityType { get { return typeof(TDBEntity); } }
        #endregion

        public void Insert(TDBEntity entity)
        {
            string columnName = string.Empty, columnNameWithParameter = string.Empty;
            foreach (var entityProperty in EntityType.GetProperties())
            {
                columnName += $"[{entityProperty.Name}],";
                columnNameWithParameter += $"@{entityProperty.Name},";
                parameterList.Add(entityProperty.Name, entityProperty.GetValue(entity));
            }
            columnName = columnName.Remove(columnName.Length - 1);
            columnNameWithParameter = columnNameWithParameter.Remove(columnNameWithParameter.Length - 1);
            runQuery = $"INSERT INTO dbo.[{EntityType.Name}] ({columnName}) VALUES ({columnNameWithParameter})";
        }

        public void Update(TDBEntity entity)
        {
            string columnName = string.Empty, columnNameWithParameter = string.Empty;
            foreach (var entityProperty in EntityType.GetProperties())
            {
                columnName += $"[{entityProperty.Name}],";
                columnNameWithParameter += $"@{entityProperty.Name},";
                parameterList.Add(entityProperty.Name, entityProperty.GetValue(entity));
            }
            columnName = columnName.Remove(columnName.Length - 1);
            columnNameWithParameter = columnNameWithParameter.Remove(columnNameWithParameter.Length - 1);
            runQuery = $"INSERT INTO [{EntitySchema}].[{EntityType.Name}] ({columnName}) VALUES ({columnNameWithParameter})";
        }

        public void Delete(TDBEntity entity)
        {
            string whereClause = string.Empty;
            foreach (var entityProperty in EntityType.GetProperties())
            {
                if(entityProperty.GetCustomAttributes(typeof(PRIMARY_KEY), false).FirstOrDefault() != null)
                {
                    whereClause = $"[{entityProperty.Name}] = @{entityProperty.Name}";
                    parameterList.Add(entityProperty.Name, entityProperty.GetValue(entity));
                }
            }
            runQuery = $"DELETE FROM [{EntitySchema}].[{EntityType.Name}] WHERE {whereClause}";
        }
       
    }
}
