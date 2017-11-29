using System;
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
        DB_Operation dB_Operation = new DB_Operation(typeof(TDBEntity).Name);
        List<DBTableModel> tableList;
        List<string> columnList;
        public CreateEverything()
        {
            this.tableList = this.dB_Operation.TableListOnDB();
            Tuple<string,string> datas = GetMSSQLQueries();
            this.dB_Operation.CreateDatabase();
            this.dB_Operation.CreateSettingTable(datas.Item2);
            this.dB_Operation.CreateTable(datas.Item1);

        }

        public Tuple<string,string> GetMSSQLQueries()
        {
            Type entityType;
            dynamic entityInformation;
            string entityColumnsMSSQL = string.Empty, entityColumnMSSQLType = string.Empty, createTableMSSQLQuery = string.Empty,
                   entityColumnsXML = string.Empty, entityColumnXML = string.Empty, createXMLObjectQuery = string.Empty;
            List<dynamic> implementedTableEntities = (from property in EntityType.GetProperties()
                                                 from genericArguments in property.PropertyType.GetGenericArguments()
                                                 where genericArguments.CustomAttributes.FirstOrDefault().AttributeType.Equals(typeof(TableAttribute))
                                                 select Activator.CreateInstance(genericArguments)).ToList();
            createXMLObjectQuery = "<Classes>";
            foreach (var entity in implementedTableEntities)
            {
                
                entityType = entity.GetType();
                entityInformation = entityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();

                string createOrAlterTable = "CREATE";
                DBTableModel willBeDeletedTableInTableList = tableList.FirstOrDefault(x => x.TableName == entityInformation.TableName && x.SchemaName == entityInformation.SchemaName);
                if (willBeDeletedTableInTableList != null)
                {
                    createOrAlterTable = "ALTER";
                    tableList.Remove(willBeDeletedTableInTableList);
                }

                columnList = dB_Operation.ColumnListOnTable(entityType.Name);

                foreach (var entityColumn in entityType.GetProperties())
                {
                    string addOrDropOrAlter = "ADD";
                    if (columnList.Where(x => x == entityColumn.Name).Count() > 0)
                        addOrDropOrAlter = "ALTER COLUMN";
                    if (columnList.Where(x => x == entityColumn.Name).Count() > 0)
                        addOrDropOrAlter = "ALTER COLUMN";

                    entityColumnsMSSQL += new CSHARP_To_MSSQL().GetSQLQueryFormat(entityColumn) + ", ";
                    entityColumnsXML += new CSHARP_To_MSSQL().GetXMLDataFormat(entityColumn);
                }
                entityColumnsMSSQL = entityColumnsMSSQL.Remove(entityColumnsMSSQL.Length - 2);
                createTableMSSQLQuery += $"{createOrAlterTable} TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ({entityColumnsMSSQL}) ";
                createXMLObjectQuery += $"<{entityType.Name}>{entityColumnsXML}</{entityType.Name}>";
                // DROP edilecek tablolar
                if (columnList.Count() > 0)
                {
                    foreach (DBTableModel table in tableList)
                    {
                        createTableMSSQLQuery += $"DROP TABLE [{table.SchemaName}].[{table.TableName}] ";
                    }
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
