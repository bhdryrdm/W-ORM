using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;
using W_ORM.Layout.Query.Evaluator;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.MSSQL
{
    public class MSSQLProviderContext<TEntityClass,TContextName> : BaseContext<TContextName>, IDB_CRUD_Operation<TEntityClass>
    {
        public static string whereCondition;

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
                if (entityProperty.GetCustomAttributes(typeof(AUTO_INCREMENT), false).FirstOrDefault() != null)
                {
                    continue;
                }
                columnName += $"[{entityProperty.Name}],";
                columnNameWithParameter += $"@{entityProperty.Name},";
                parameterList.Add(entityProperty.Name, entityProperty.GetValue(entity));
            }
            columnName = columnName.Remove(columnName.Length - 1);
            columnNameWithParameter = columnNameWithParameter.Remove(columnNameWithParameter.Length - 1);
            runQuery = $"INSERT INTO [{EntitySchema}].[{EntityType.Name}] ({columnName}) VALUES ({columnNameWithParameter});";
        }

        public void Update(TEntityClass entity)
        {
            string columnNameAndValue = string.Empty;
            foreach (var entityProperty in EntityType.GetProperties())
            {
                if (entityProperty.GetCustomAttributes(typeof(AUTO_INCREMENT), false).FirstOrDefault() != null)
                {
                    continue;
                }
                columnNameAndValue += $"{entityProperty.Name} = @{entityProperty.Name},";
                parameterList.Add(entityProperty.Name, entityProperty.GetValue(entity));
            }
            columnNameAndValue = columnNameAndValue.Remove(columnNameAndValue.Length - 1);
            runQuery = $"UPDATE [{EntitySchema}].[{EntityType.Name}] SET {columnNameAndValue} ";
            runQuery += !string.IsNullOrEmpty(whereCondition) ? $"WHERE {whereCondition}" : "";
        }

        public void Delete(TEntityClass entity)
        {
            runQuery = $"DELETE FROM [{EntitySchema}].[{EntityType.Name}] WHERE {whereCondition}";
        }

        public List<TEntityClass> ToList()
        {
            runQuery = $"SELECT * FROM [{EntitySchema}].[{EntityType.Name}]";
            return base.GetListFromDB<TEntityClass>();
        }

        public TEntityClass FirstOrDefault(Expression<Func<TEntityClass, object>> predicate)
        {
            runQuery = $"SELECT TOP 1 * FROM [{EntitySchema}].[{EntityType.Name}]";
            whereCondition = new QueryTranslator().Translate(Evaluator.PartialEval(predicate));
            runQuery += !string.IsNullOrEmpty(whereCondition) ? " WHERE " + whereCondition : "";
            return base.GetItemFromDB<TEntityClass>();
        }

        public List<TEntityClass> Where(Expression<Func<TEntityClass, object>> predicate)
        {
            runQuery = $"SELECT * FROM [{EntitySchema}].[{EntityType.Name}]";
            string whereCondition = new QueryTranslator().Translate(Evaluator.PartialEval(predicate));
            runQuery += !string.IsNullOrEmpty(whereCondition) ? " WHERE " + whereCondition : "";
            return base.GetListFromDB<TEntityClass>();
        }
    }
}
