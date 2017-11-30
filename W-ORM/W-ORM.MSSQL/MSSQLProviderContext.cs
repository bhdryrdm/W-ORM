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

namespace W_ORM.MSSQL
{
    public class MSSQLProviderContext<TDBEntity> : BaseContext, IDB_CRUD_Operation<TDBEntity>
    {

        protected string EntitySchema
        {
            get
            {
                dynamic entitySchema = EntityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
                return entitySchema.SchemaName ? entitySchema.SchemaName : "dbo"; 
            }
        } 
        protected Type EntityType
        {
            get
            {
                return typeof(TDBEntity);
            }
        }

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
            throw new NotImplementedException();
        }
        public void Delete(TDBEntity entity)
        {
            throw new NotImplementedException();
        }

        

    }
}
