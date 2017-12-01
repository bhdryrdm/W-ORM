using System;
using System.Collections.Generic;
using System.Linq;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.Query;

namespace W_ORM.MSSQL
{
    public class CreateEverything<TDBEntity> : IEntityClassQueryGenerator<TDBEntity> where TDBEntity : class
    {
        public static string contextName = typeof(TDBEntity).Name;
        List<DBTableModel> tableList = new DB_Operation(contextName).TableListOnDB();
        List<string> columnList;

        public Tuple<string, string> EntityClassQueries()
        {
            Type entityType;
            dynamic entityInformation;
            string entityColumnsMSSQL = string.Empty, entityColumnMSSQLType = string.Empty, createTableMSSQLQuery = string.Empty,
                   entityColumnsXML = string.Empty, entityColumnXML = string.Empty, createXMLObjectQuery = string.Empty;
            List<dynamic> implementedTableEntities = (from property in typeof(TDBEntity).GetProperties()
                                                      from genericArguments in property.PropertyType.GetGenericArguments()
                                                      where genericArguments.CustomAttributes.FirstOrDefault().AttributeType.Equals(typeof(TableAttribute))
                                                      select Activator.CreateInstance(genericArguments)).ToList();
            // Tablolar bazında dönülüyor // Classlarda dönüyorum
            createXMLObjectQuery = "<Classes>";
            foreach (var entity in implementedTableEntities)
            {

                entityType = entity.GetType();
                entityInformation = entityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();

                columnList = new DB_Operation(contextName).ColumnListOnTable(entityType.Name);

                // Sütunlar bazında dönülüyor // Property dönüyorum
                foreach (var entityColumn in entityType.GetProperties())
                {
                    var columnInformation = new CSHARP_To_MSSQL().GetSQLQueryFormat(entityColumn);
                    if (columnList.Where(x => x == entityColumn.Name).Count() == 0 && columnList.Count() > 0)
                    {
                        createTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ADD {columnInformation} ";
                    }

                    if (columnList.Where(x => x == entityColumn.Name).Count() > 0)
                    {
                        columnList.Remove(entityColumn.Name);
                        createTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ALTER COLUMN {columnInformation} ";
                    }

                    if (tableList.Where(x => x.SchemaName == entityInformation.SchemaName && x.TableName == entityInformation.TableName).Count() == 0)
                    {
                        entityColumnsMSSQL += columnInformation + ", ";
                    }

                    entityColumnsXML += new CSHARP_To_MSSQL().GetXMLDataFormat(entityColumn);
                }
                if (!string.IsNullOrEmpty(entityColumnsMSSQL))
                {
                    entityColumnsMSSQL = entityColumnsMSSQL.Remove(entityColumnsMSSQL.Length - 2);
                    createTableMSSQLQuery += $"CREATE TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ({entityColumnsMSSQL}) ";
                }

                createXMLObjectQuery += $"<{entityType.Name}>{entityColumnsXML}</{entityType.Name}>";
                var tableInformation = new DBTableModel { SchemaName = entityInformation.SchemaName, TableName = entityInformation.TableName };
                if (tableList.Where(x => x.SchemaName == tableInformation.SchemaName && x.TableName == tableInformation.TableName).Count() > 0)
                {
                    tableList.Remove(tableInformation);
                }

                // DROP edilecek sütunlar
                if (columnList.Count() > 0)
                {
                    string columnNames = string.Empty;
                    foreach (string columnName in columnList)
                    {
                        columnNames = $"{columnName}, ";
                    }
                    columnNames = columnNames.Remove(columnNames.Length - 2);
                    createTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] DROP COLUMN {columnNames}";
                }
                entityColumnsMSSQL = string.Empty;
                entityColumnsXML = string.Empty;
            }
            createXMLObjectQuery += "</Classes>";
            // DROP edilecek tablolar
            if (tableList.Count() > 0)
            {
                foreach (DBTableModel table in tableList)
                {
                    createTableMSSQLQuery += $"DROP TABLE [{table.SchemaName}].[{table.TableName}] ";
                }
            }
            return Tuple.Create(createTableMSSQLQuery, createXMLObjectQuery);
        }
    }
}
