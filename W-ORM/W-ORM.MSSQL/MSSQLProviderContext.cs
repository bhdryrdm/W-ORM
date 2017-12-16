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
        private string versionQuery = "SELECT TOP 1 Version FROM [dbo].[__WORM__Configuration] ORDER BY Version DESC";

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

        #region IDB_CRUD_Operation
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
            runVersionQuery = versionQuery;
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
            runVersionQuery = versionQuery;
        }

        public void Delete(TEntityClass entity)
        {
            runQuery = $"DELETE FROM [{EntitySchema}].[{EntityType.Name}] WHERE {whereCondition}";
            runVersionQuery = versionQuery;
        }
        #endregion

        #region IDB_CRUD_Helper_Operation
        public List<TEntityClass> ToList()
        {
            runQuery = $"SELECT * FROM [{EntitySchema}].[{EntityType.Name}]";
            return base.GetListFromDB<TEntityClass>();
        }

        public List<TEntityClass> ToPaginateList(Expression<Func<TEntityClass, object>> predicate = null, string orderByColumn = "", int pageSize = 10, int requestedPageNumber = 1)
        {
            string whereCondition = new QueryTranslator().Translate(Evaluator.PartialEval(predicate));
            runQuery = $"SELECT * FROM (SELECT * , ROW_NUMBER() OVER (ORDER BY {(!string.IsNullOrEmpty(orderByColumn) ? orderByColumn : typeof(TEntityClass).GetProperties().FirstOrDefault().Name)}) " +
                       $"AS ROW  FROM [{EntitySchema}].[{EntityType.Name}] {(!string.IsNullOrEmpty(whereCondition) ? " WHERE " + whereCondition : "")} ) " +
                       $"AS PAGED WHERE ROW > {(requestedPageNumber - 1) * pageSize} AND ROW <= {requestedPageNumber * pageSize}";
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
        #endregion
    }
}
