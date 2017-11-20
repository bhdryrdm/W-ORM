﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;

namespace W_ORM.MSSQL
{
    public class CreateEverything<TDBEntity> : MSSQLProviderContext<TDBEntity>
    {
        /// <summary>
        /// Create Database and setting table 
        /// </summary>
        /// <param name="databaseName">Database Name</param>
        public CreateEverything(string databaseName)
        {
            DB_Operation dB_Operation = new DB_Operation(typeof(TDBEntity).Name,databaseName);
            Tuple<string,string> datas = GetMSSQLQueries();
            dB_Operation.CreateDatabase();
            dB_Operation.CreateSettingTable(datas.Item2);
            dB_Operation.CreateTable(datas.Item1);
        }

        public Tuple<string,string> GetMSSQLQueries()
        {
            Type entityType;
            dynamic entityInformation;
            string entityColumnsMSSQL = string.Empty, entityColumnMSSQLType = string.Empty, createTableMSSQLQuery = string.Empty,
                   entityColumnsXML = string.Empty, entityColumnXML = string.Empty, createXMLObjectQuery = string.Empty;
            List<dynamic> implementedEntities = (from property in EntityType.GetProperties()
                                                 from genericArguments in property.PropertyType.GetGenericArguments()
                                                 where genericArguments.BaseType.Equals(typeof(ModelBase))
                                                 select Activator.CreateInstance(genericArguments)).ToList();
            createXMLObjectQuery = "<Classes>";
            foreach (var entity in implementedEntities)
            {
                entityType = entity.GetType();
                entityInformation = entityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();

                foreach (var entityColumn in entityType.GetProperties())
                {
                    entityColumnsMSSQL += new CSHARP_To_MSSQL().GetSQLQueryFormat(entityColumn) + ", ";

                    entityColumnsXML += new CSHARP_To_MSSQL().GetXMLDataFormat(entityColumn);
                }
                entityColumnsMSSQL = entityColumnsMSSQL.Remove(entityColumnsMSSQL.Length - 2);
                createTableMSSQLQuery += $"CREATE TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ({entityColumnsMSSQL}) ";


                createXMLObjectQuery += $"<{entityType.Name}>" +
                                    $"{entityColumnsXML}" +
                                    $"</{entityType.Name}>";

                entityColumnsMSSQL = string.Empty;
                entityColumnsXML = string.Empty;
            }
            createXMLObjectQuery += "</Classes>";

            return Tuple.Create(createTableMSSQLQuery, createXMLObjectQuery);
        }
    }
}