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
                   dropConstraintListMSSQLQuery = string.Empty,
                   entityColumnsXML = string.Empty,
                   entityColumnXML = string.Empty,
                   createXMLObjectQuery = string.Empty;
            List<string> dropConstraintList = new List<string>();
            #endregion

            #region Entity Class(Tablolar)larının property(sütun) olarak tanımlanmış olduğu Context(Database) sınıfından Entity Class(tablo)lar listelenir
            List<dynamic> implementedTableEntities = (from property in typeof(TDBEntity).GetProperties()
                                                      from genericArguments in property.PropertyType.GetGenericArguments()
                                                      where genericArguments.CustomAttributes.FirstOrDefault() != null &&
                                                            genericArguments.CustomAttributes.FirstOrDefault().AttributeType.Equals(typeof(TableAttribute)) 
                                                      orderby genericArguments.CustomAttributes.FirstOrDefault().NamedArguments.FirstOrDefault(x => x.MemberName == "OrdinalPosition").TypedValue.Value
                                                      select Activator.CreateInstance(genericArguments)).ToList();
            #endregion
           
            #region Creating SQL Server Queries

            #region Veritabanı versiyonu için XML verisi ve Create&Alter Tabloları ve Sütunları sorguları oluşturulur
            createXMLObjectQuery = "<Classes>"; // Veritabanında versiyonlama için kullanılacak XML bilgisinin başlangıcı
            foreach (var entity in implementedTableEntities) // Entity Class yani Veritabaındaki Tabloları oluşturmak için döngü başlatılır
            {
                entityType = entity.GetType();
                entityInformation = entityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(); // Entity Class üzerindeki Table Attribute üzerinden bilgiler okunur

                columnList = new DB_Operation(contextName).ColumnListOnTable(entityType.Name); // Veritabanından ilgili tablonun tüm sütunları çekilir

                // Veritabanında Tablonun var olup olmadığı kontrol edilir Tablo Varsa ? ALTER çalışır CREATE çalışmaz : ALTER çalışmaz CREATE çalışır
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

                    if (columnList.Where(x => x == entityColumn.Name).Count() > 0)
                    {
                        columnList.Remove(entityColumn.Name); // Tablo içerisindeki sütun listesinden işlem yapılan sütun çıkartılır (DROP edilmeyecektir)
                        string constraintName = new DB_Operation(contextName).ConstraintNameByTableAndColumnName(entityInformation.SchemaName, entityInformation.TableName, entityColumn.Name);
                        if (!string.IsNullOrEmpty(constraintName))
                        {
                            dropConstraintList.Add($"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] DROP CONSTRAINT {constraintName} ");
                        }

                        dynamic isEntityColumnAutoIncrement = entityColumn.GetCustomAttributes(typeof(AUTO_INCREMENT), false).FirstOrDefault();
                        if (isEntityColumnAutoIncrement != null)
                        {
                            alterTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] DROP COLUMN {entityColumn.Name} ";
                            columnInformation = columnInformation.Replace("PRIMARY KEY", "");
                            alterTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ADD {columnInformation} ";
                            columnInformation = columnInformation.Replace($"IDENTITY({isEntityColumnAutoIncrement.StartNumber},{isEntityColumnAutoIncrement.Increase})", "");

                        }

                        var isEntityColumnPrimaryKey = entityColumn.GetCustomAttributes(typeof(PRIMARY_KEY), false).FirstOrDefault();
                        if (isEntityColumnPrimaryKey != null)
                        {
                            columnInformation = columnInformation.Replace("PRIMARY KEY", "");
                            alterTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ADD PRIMARY KEY ({entityColumn.Name}) ";
                        }

                        dynamic isEntityColumnForeignKey = entityColumn.GetCustomAttributes(typeof(FOREIGN_KEY), false).FirstOrDefault();
                        if (isEntityColumnForeignKey != null)
                        {
                            columnInformation = columnInformation.Replace($"FOREIGN KEY REFERENCES {isEntityColumnForeignKey.ClassName}({isEntityColumnForeignKey.PropertyName})", "");
                            alterTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] " +
                                                    $"ADD FOREIGN KEY({ entityColumn.Name}) REFERENCES {isEntityColumnForeignKey.ClassName}({isEntityColumnForeignKey.PropertyName}) ";
                        }

                        alterTableMSSQLQuery += $"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ALTER COLUMN {columnInformation} ";
                    }
                    #endregion

                    #region Entity Class(tablo) Veritabanında yoksa oluşturulacak olan sütunların MSSQL Query generate edilir
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
                createXMLObjectQuery += $"<{entityType.Name} SchemaName=\"{entityInformation.SchemaName}\" TableName=\"{entityInformation.TableName}\" OrdinalPosition=\"{entityInformation.OrdinalPosition}\">" +
                                        $"{entityColumnsXML}" +
                                        $"</{entityType.Name}>";

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
                        string constraintName = new DB_Operation(contextName).ConstraintNameByTableAndColumnName(entityInformation.SchemaName, entityInformation.TableName, columnName);
                        if (!string.IsNullOrEmpty(constraintName))
                        {
                            dropConstraintList.Add($"ALTER TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] DROP CONSTRAINT {constraintName} ");
                        }
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

            #region Constraintler kaldırılır
            dropConstraintList.Reverse();
            foreach (string item in dropConstraintList)
            {
                dropConstraintListMSSQLQuery += item;
            }
            #endregion

            #endregion

            //                      DROP TABLES             CREATE TABLES                         ALTER TABLES                         XML QUERY
            return Tuple.Create(dropTableMSSQLQuery + createTableMSSQLQuery + (dropConstraintListMSSQLQuery + alterTableMSSQLQuery), createXMLObjectQuery);
        }
    }
}
