using System;
using System.Collections.Generic;
using System.Linq;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.Query;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.MSSQL
{
    public class CreateEverything<TDBEntity> : IEntityClassQueryGenerator<TDBEntity>
    {
        public static string contextName = typeof(TDBEntity).Name;
        List<DBTableModel> tableList = new DB_Operation(contextName).TableListOnDB();
        List<string> columnList;

        /// <summary>
        /// SQL Server için oluşturulacak Query ler generate edilir
        /// </summary>
        /// <returns></returns>
        public Tuple<string, string> EntityClassQueries()
        {
            #region Field Defination
            Type entityType;
            dynamic entityInformation;
            string entityColumnsMSSQL = string.Empty,
                   createTableMSSQLQuery = string.Empty,
                   alterTableMSSQLQuery = string.Empty,
                   dropTableMSSQLQuery = string.Empty,
                   entityColumnsXML = string.Empty,
                   entityColumnXML = string.Empty,
                   createXMLObjectQuery = string.Empty;
            #endregion

            #region Entity Classlarının property olarak tanımlanmış olduğu Context sınıfından Class lar generate edilir
            List<dynamic> implementedTableEntities = (from property in typeof(TDBEntity).GetProperties()
                                                      from genericArguments in property.PropertyType.GetGenericArguments()
                                                      where genericArguments.CustomAttributes.FirstOrDefault().AttributeType.Equals(typeof(TableAttribute))
                                                      select Activator.CreateInstance(genericArguments)).ToList();
            #endregion

            #region Creating SQL Server Queries

                #region Veritabanı versiyonu için XML verisi ve Create&Alter Tabloları ve Sütunları
                createXMLObjectQuery = "<Classes>"; // Veritabanında versiyonlama için kullanılacak XML bilgisinin başlangıcı
                foreach (var entity in implementedTableEntities) // Entity Class yani Veritabaındaki Tabloları oluşturmak için döngü başlatılır
                {
                    entityType = entity.GetType();
                    entityInformation = entityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(); // Entity Class üzerindeki Table Attribute üzerinden bilgiler okunur

                    columnList = new DB_Operation(contextName).ColumnListOnTable(entityType.Name); // Veritabanından ilgili tablonun tüm sütunları çekilir

                    // Veritabanında Tablonun var olup olmadığı kontrol edilir Tablo Varsa ? ALTER çalışmaz CREATE çalışır : ALTER çalışır CREATE çalışmaz
                    bool tableExistOnDb = tableList.FirstOrDefault(x => x.SchemaName == entityInformation.SchemaName && x.TableName == entityInformation.TableName) != null ? true : false;

                    foreach (var entityColumn in entityType.GetProperties()) // Entity Class içerisindeki property ler yani Sütunları oluşturmak için döngü başlatılır
                    {
                        // Property inin Custom Attributelerine bakarak MSSQL Query generate edilir
                        var columnInformation = new CSHARP_To_MSSQL().GetSQLQueryFormat(entityColumn);

                        #region Entity Class içerisinde oluşturulmuş yeni bir property varsa yani sütun ve Entity Class yani Tablo Veritabanında varsa
                        if (columnList.Where(x => x == entityColumn.Name).Count() == 0 && tableExistOnDb)
                        {
                            alterTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ADD {columnInformation} ";
                        }
                    #endregion

                        #region Entity Class içerisinde var olup değişikliğe uğramış bir property
                        // TODO : PrimaryKey ve Foreign Key yapılacak
                        //var isEntityColumnPrimaryKey = entityColumn.GetCustomAttributes(typeof(PRIMARY_KEY), false).FirstOrDefault();
                        if (columnList.Where(x => x == entityColumn.Name).Count() > 0 /* && isEntityColumnPrimaryKey == null*/)
                        {
                            columnList.Remove(entityColumn.Name); // Tablo içerisindeki sütun listesinden işlem yapılan sütun çıkartılır (DROP edilmeyecektir)
                            alterTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ALTER COLUMN {columnInformation} ";
                        }
                        #endregion

                        #region Entity Class yani Tablo Veritabanında yoksa oluşturulacak olan sütunların MSSQL Query generate edilir
                        if (tableList.Where(x => x.SchemaName == entityInformation.SchemaName && x.TableName == entityInformation.TableName).Count() == 0)
                        {
                            entityColumnsMSSQL += columnInformation + ", ";
                        }
                        #endregion

                        #region XMLQuery Sütunları generate edilir
                        entityColumnsXML += new CSHARP_To_MSSQL().GetXMLDataFormat(entityColumn);
                        #endregion
                    }

                    if (!string.IsNullOrEmpty(entityColumnsMSSQL))
                    {
                        entityColumnsMSSQL = entityColumnsMSSQL.Remove(entityColumnsMSSQL.Length - 2);
                        createTableMSSQLQuery += $"CREATE TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ({entityColumnsMSSQL}) ";
                    }

                    // Veritabanında versiyonlama için kullanılacak XML bilgisinin gövdesi
                    createXMLObjectQuery += $"<{entityType.Name}>{entityColumnsXML}</{entityType.Name}>";

                    #region Tablo tableList listesinden çıkartılır
                    DBTableModel tableInformation = tableList.FirstOrDefault(x => x.SchemaName == entityInformation.SchemaName && x.TableName == entityInformation.TableName);
                    if (tableInformation != null) // Veritabanında Tablo zaten var ise güncelleme yapılıyordur (DROP edilmeyecektir)
                    {
                        tableList.Remove(tableInformation);
                    }
                    #endregion

                    #region DROP edilecek sütunlar 
                    if (columnList.Count() > 0)
                    {
                        string columnNames = string.Empty;
                        foreach (string columnName in columnList)
                        {
                            columnNames += $"{columnName}, ";
                        }
                        columnNames = columnNames.Remove(columnNames.Length - 2);
                        alterTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] DROP COLUMN {columnNames} ";
                    }
                    #endregion

                    entityColumnsMSSQL = string.Empty;
                    entityColumnsXML = string.Empty;
                } // Entity Class yani Veritabaındaki Tabloları oluşturmak için döngü sonlandırılır
                createXMLObjectQuery += "</Classes>"; // Veritabanında versiyonlama için kullanılacak XML bilgisinin bitişi
                #endregion

                #region DROP edilecek tablolar
                if (tableList.Count() > 0)
                {
                    #region __WORM__Configuration dosyası silenecek tablolar listesinden kaldırılır
                    DBTableModel wormTable = tableList.FirstOrDefault(x => x.SchemaName == "dbo" && x.TableName == "__WORM__Configuration");
                    if (wormTable != null)
                        tableList.Remove(wormTable);
                    #endregion

                    // Tablo listesinde kalan herhangi bir kayıt Veritabanından silinecek tabloyu işaret eder.
                    // Yani Kod tarafında bu tabloya ait Entity Class silinmiştir.
                    foreach (DBTableModel table in tableList)
                    {
                        dropTableMSSQLQuery += $"DROP TABLE [{table.SchemaName}].[{table.TableName}] ";
                    }
                }
                #endregion

            #endregion

            //                      DROP TABLES             CREATE TABLES          ALTER TABLES           XML QUERY
            return Tuple.Create( dropTableMSSQLQuery + createTableMSSQLQuery + alterTableMSSQLQuery, createXMLObjectQuery);
        }
    }
}
