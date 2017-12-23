using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;
using W_ORM.Layout.Query.Evaluator;
using W_ORM.MYSQL.Attributes;

namespace W_ORM.MYSQL
{
    public class MYSQLProviderContext<TEntityClass, TContextName> : BaseContext<TContextName>, IDB_CRUD_Operation<TEntityClass>
    {
        public static string whereCondition;
        private string versionQuery = "SELECT Version FROM [dbo].[__WORM__Configuration] ORDER BY Version DESC LIMIT 1";

        #region Entity Type
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
                columnName += $"{entityProperty.Name},";
                columnNameWithParameter += $"@{entityProperty.Name},";
                parameterList.Add(entityProperty.Name, entityProperty.GetValue(entity));
            }
            columnName = columnName.Remove(columnName.Length - 1);
            columnNameWithParameter = columnNameWithParameter.Remove(columnNameWithParameter.Length - 1);
            runQuery = $"INSERT INTO {EntityType.Name} ({columnName}) VALUES ({columnNameWithParameter});";
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
            runQuery = $"UPDATE {EntityType.Name} SET {columnNameAndValue} ";
            runQuery += !string.IsNullOrEmpty(whereCondition) ? $"WHERE {whereCondition}" : "";
            runVersionQuery = versionQuery;
        }

        public void Delete(TEntityClass entity)
        {
            runQuery = $"DELETE FROM {EntityType.Name} WHERE {whereCondition}";
            runVersionQuery = versionQuery;
        }
        #endregion

        #region IDB_CRUD_Transaction_Operation
        public void Insert(TEntityClass entity,DbTransaction transaction)
        {
            parameterList = new Dictionary<string, object>();
            string columnName = string.Empty, columnNameWithParameter = string.Empty;
            foreach (var entityProperty in EntityType.GetProperties())
            {
                if (entityProperty.GetCustomAttributes(typeof(AUTO_INCREMENT), false).FirstOrDefault() != null)
                {
                    continue;
                }
                columnName += $"{entityProperty.Name},";
                columnNameWithParameter += $"@{entityProperty.Name},";
                parameterList.Add(entityProperty.Name, entityProperty.GetValue(entity));
            }
            columnName = columnName.Remove(columnName.Length - 1);
            columnNameWithParameter = columnNameWithParameter.Remove(columnNameWithParameter.Length - 1);
            runQuery = $"INSERT INTO {EntityType.Name} ({columnName}) VALUES ({columnNameWithParameter});";
            BaseTransaction(transaction);
        }

        public void Update(Expression<Func<TEntityClass, object>> predicate, TEntityClass entity, DbTransaction transaction)
        {
            string whereCondition = new QueryTranslator().Translate(Evaluator.PartialEval(predicate));
            parameterList = new Dictionary<string, object>();
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
            runQuery = $"UPDATE {EntityType.Name} SET {columnNameAndValue} ";
            runQuery += !string.IsNullOrEmpty(whereCondition) ? $"WHERE {whereCondition}" : "";
            BaseTransaction(transaction);
        }

        public void Delete(Expression<Func<TEntityClass, object>> predicate, DbTransaction transaction)
        {
            string whereCondition = new QueryTranslator().Translate(Evaluator.PartialEval(predicate));
            runQuery = $"DELETE FROM {EntityType.Name} WHERE {whereCondition}";
            BaseTransaction(transaction);
        }
        #endregion

        #region IDB_CRUD_Helper_Operation
        public List<TEntityClass> ToList()
        {
            runQuery = $"SELECT * FROM {EntityType.Name}";
            return base.GetListFromDB<TEntityClass>();
        }

        public TEntityClass FirstOrDefault(Expression<Func<TEntityClass, object>> predicate)
        {
            runQuery = $"SELECT * FROM {EntityType.Name}";
            whereCondition = new QueryTranslator().Translate(Evaluator.PartialEval(predicate));
            runQuery += !string.IsNullOrEmpty(whereCondition) ? " WHERE " + whereCondition : "";
            runQuery += " LIMIT 1";
            return base.GetItemFromDB<TEntityClass>();
        }

        public List<TEntityClass> Where(Expression<Func<TEntityClass, object>> predicate)
        {
            runQuery = $"SELECT * FROM {EntityType.Name}";
            string whereCondition = new QueryTranslator().Translate(Evaluator.PartialEval(predicate));
            runQuery += !string.IsNullOrEmpty(whereCondition) ? " WHERE " + whereCondition : "";
            return base.GetListFromDB<TEntityClass>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="orderByColumn">Sıralanması istenen Sütun</param>
        /// <param name="pageSize">Bir sayfada kaç item olacak Default : 10</param>
        /// <param name="requestedPageNumber">İstenen sayfa numarası Default : 1</param>
        /// <returns></returns>
        public List<TEntityClass> ToPaginateList(Expression<Func<TEntityClass, object>> predicate, string orderByColumn, int pageSize = 10, int requestedPageNumber = 1)
        {
            string whereCondition = new QueryTranslator().Translate(Evaluator.PartialEval(predicate));
            runQuery = $"SELECT * FROM {EntityType.Name} " +
                        $"{(!string.IsNullOrEmpty(whereCondition) ? " WHERE " + whereCondition : "")}" +
                        $"LIMIT {(requestedPageNumber - 1) * pageSize},{requestedPageNumber * pageSize} ";
            return base.GetListFromDB<TEntityClass>();
        }
        #endregion
    }
}
