using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.MSSQL
{
    public class MSSQLProviderContext<TEntityClass,TContextName> : BaseContext<TContextName>, IDB_CRUD_Operation<TEntityClass>
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

        protected Type EntityType { get { return typeof(TEntityClass); } }
        #endregion

        public void Insert(TEntityClass entity)
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

        public void Update(TEntityClass entity)
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

        public void Delete(TEntityClass entity)
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
            runQuery += $"DELETE FROM [{EntitySchema}].[{EntityType.Name}] WHERE {whereClause}";
        }

        public List<TEntityClass> ToList()
        {
            runQuery = $"SELECT * FROM [{EntitySchema}].[{EntityType.Name}]";
            return base.GetListFromDB<TEntityClass>();
        }

        public TEntityClass FirstOrDefault(Expression<Func<TEntityClass, object>> predicate)
        {
            runQuery = $"SELECT TOP 1 * FROM [{EntitySchema}].[{EntityType.Name}]";
            var binaryExpression = (BinaryExpression)((UnaryExpression)predicate.Body).Operand;
            var left = Expression.Lambda<Func<TEntityClass, object>>(Expression.Convert(binaryExpression.Left, typeof(object)), predicate.Parameters[0]);
            return base.GetItemFromDB<TEntityClass>();
        }

        public List<TEntityClass> Where(Expression<Func<TEntityClass, object>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
