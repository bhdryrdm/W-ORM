using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.Query;
using W_ORM.MYSQL.Attributes;

namespace W_ORM.MYSQL
{
    public class CreateDatabase<TDBEntity> : IEntityClassQueryGenerator<TDBEntity>
    {
        public static string contextName = typeof(TDBEntity).Name;
        List<DBTableModel> tableList = new DB_Operation(contextName).TableListOnDB();
        List<string> columnList;

        /// <summary>
        /// MYSQL Server için oluşturulacak Query ler generate edilir
        /// </summary>
        /// <returns></returns>
        public Tuple<string, string> EntityClassQueries()
        {
            #region Field Defination
            Type entityType;
            dynamic entityInformation;
            string entityColumnsMYSQL = string.Empty,
                   createTableMYSQLQuery = string.Empty,
                   alterTableMYSQLQuery = string.Empty,
                   dropTableMYSQLQuery = string.Empty,
                   dropConstraintListMYSQLQuery = string.Empty,
                   entityColumnsXML = string.Empty,
                   entityColumnXML = string.Empty,
                   createXMLObjectQuery = string.Empty;
            List<string> dropConstraintList = new List<string>();
            #endregion

            #region Entity Classlarının property olarak tanımlanmış olduğu Context sınıfından Class lar generate edilir
            List<dynamic> implementedTableEntities = (from property in typeof(TDBEntity).GetProperties()
                                                      from genericArguments in property.PropertyType.GetGenericArguments()
                                                      where genericArguments.CustomAttributes.FirstOrDefault() != null &&
                                                             genericArguments.CustomAttributes.FirstOrDefault().AttributeType.Equals(typeof(TableAttribute))
                                                      orderby genericArguments.CustomAttributes.FirstOrDefault().NamedArguments.FirstOrDefault(x => x.MemberName == "OrdinalPosition").TypedValue.Value
                                                      select Activator.CreateInstance(genericArguments)).ToList();
            #endregion

            #region Creating SQL Server Queries

            #region Veritabanı versiyonu için XML verisi ve Create&Alter Tabloları ve Sütunları
            createXMLObjectQuery = "<Classes>"; // Veritabanında versiyonlama için kullanılacak XML bilgisinin başlangıcı
            foreach (var entity in implementedTableEntities) // Entity Classlar(Tablo) oluşturmak için döngü başlatılır
            {
                entityType = entity.GetType();
                entityInformation = entityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(); // Entity Class üzerindeki Table Attribute üzerinden Schema ve Tablo İsmi okunur

                columnList = new DB_Operation(contextName).ColumnListOnTable(entityType.Name); // Veritabanından ilgili tablonun tüm sütunları çekilir

                // Veritabanında Tablonun var olup olmadığı kontrol edilir Tablo Varsa ? ALTER çalışır CREATE çalışmaz : ALTER çalışmaz CREATE çalışır
                bool tableExistOnDb = tableList.FirstOrDefault(x => x.TableName == entityInformation.TableName.ToLower()) != null ? true : false;

                foreach (var entityColumn in entityType.GetProperties()) // Entity Class içerisindeki property ler yani Sütunları oluşturmak için döngü başlatılır
                {
                    // Property inin Custom Attributelerine bakarak MYSQL Query generate edilir
                    var columnInformation = new CSHARP_To_MYSQL().GetSQLQueryFormat(entityColumn);
                    var isEntityColumnPrimaryKey = entityColumn.GetCustomAttributes(typeof(PRIMARY_KEY), false).FirstOrDefault();
                    dynamic isEntityColumnForeignKey = entityColumn.GetCustomAttributes(typeof(FOREIGN_KEY), false).FirstOrDefault();
                    
                    #region Entity Class(Tablo) içerisinde oluşturulmuş yeni bir property(sütun/column) varsa ve Entity Class(Tablo) Veritabanında varsa
                    if (columnList.Where(x => x == entityColumn.Name).Count() == 0 && tableExistOnDb)
                    {
                        if (isEntityColumnPrimaryKey != null && isEntityColumnForeignKey != null)
                        {
                            if(entityColumn.GetCustomAttributes(typeof(FOREIGN_KEY), false).FirstOrDefault() != null)
                                alterTableMYSQLQuery += $"ALTER TABLE {entityInformation.TableName} AUTO_INCREMENT=1;";
                            alterTableMYSQLQuery += $"ALTER TABLE {entityInformation.TableName} ADD {columnInformation};";
                        }
                        if (isEntityColumnPrimaryKey != null)
                        {
                            columnInformation = columnInformation.Replace($",PRIMARY KEY ({entityColumn.Name})", "");
                            alterTableMYSQLQuery += $"ALTER TABLE {entityInformation.TableName} ADD {columnInformation};";
                            alterTableMYSQLQuery += $"ALTER TABLE {entityInformation.TableName} ADD PRIMARY KEY ({entityColumn.Name});";
                        }
                        if(isEntityColumnForeignKey != null)
                        {
                            columnInformation = columnInformation.Replace($",FOREIGN KEY ({isEntityColumnForeignKey.PropertyName}) REFERENCES {isEntityColumnForeignKey.ClassName}({isEntityColumnForeignKey.PropertyName})", "");
                            alterTableMYSQLQuery += $"ALTER TABLE {entityInformation.TableName} ADD {columnInformation}; ";
                            alterTableMYSQLQuery += $"ALTER TABLE {entityInformation.TableName} " +
                                                    $"ADD FOREIGN KEY({ entityColumn.Name}) REFERENCES {isEntityColumnForeignKey.ClassName}({isEntityColumnForeignKey.PropertyName});";
                        }
                    }
                    #endregion

                    #region Entity Class içerisinde var olup değişikliğe uğramış bir property
                    if (columnList.Where(x => x == entityColumn.Name).Count() > 0)
                    {
                        columnList.Remove(entityColumn.Name); // Tablo içerisindeki sütun listesinden işlem yapılan sütun çıkartılır (DROP edilmeyecektir)
                        Tuple<string,string> constraintTupleData = new DB_Operation(contextName).ConstraintNameByTableAndColumnName(entityInformation.TableName, entityColumn.Name);
                        if (!string.IsNullOrEmpty(constraintTupleData.Item1))
                        {
                            if(isEntityColumnPrimaryKey != null)
                            {
                                dropConstraintList.Add($"ALTER TABLE {entityInformation.TableName} DROP PRIMARY KEY;");

                                // MYSQL Auto Increment olan bir sütunu DROP edemiyor
                                // Önce Sütundan Auto Increment kaldırılır daha sonra PRIMARY KEY DROP edilir
                                columnInformation = columnInformation.Replace($",PRIMARY KEY ({entityColumn.Name})", "");
                                columnInformation = columnInformation.Replace($"AUTO_INCREMENT", "");
                                dropConstraintList.Add($"ALTER TABLE {entityInformation.TableName} MODIFY COLUMN {columnInformation};");

                            }
                            else
                                dropConstraintList.Add($"ALTER TABLE {entityInformation.TableName} DROP FOREIGN KEY {constraintTupleData.Item1}; ");
                        }

                        if (isEntityColumnPrimaryKey != null)
                        {
                            columnInformation = columnInformation.Replace($",PRIMARY KEY ({entityColumn.Name})", "");
                            alterTableMYSQLQuery += $"ALTER TABLE {entityInformation.TableName} ADD PRIMARY KEY ({entityColumn.Name});";
                        }
                        
                        if (isEntityColumnForeignKey != null)
                        {
                            columnInformation = columnInformation.Replace($",FOREIGN KEY ({isEntityColumnForeignKey.PropertyName}) REFERENCES {isEntityColumnForeignKey.ClassName}({isEntityColumnForeignKey.PropertyName})", "");
                            alterTableMYSQLQuery += $"ALTER TABLE {entityInformation.TableName} " +
                                                    $"ADD FOREIGN KEY({ entityColumn.Name}) REFERENCES {isEntityColumnForeignKey.ClassName}({isEntityColumnForeignKey.PropertyName});";
                        }

                        alterTableMYSQLQuery += $"ALTER TABLE {entityInformation.TableName} MODIFY COLUMN {columnInformation};";
                    }
                    #endregion

                    #region Entity Class yani Tablo Veritabanında yoksa oluşturulacak olan sütunların MSSQL Query generate edilir
                    if (tableList.Where(x => x.TableName == entityInformation.TableName.ToLower()).Count() == 0)
                    {
                        entityColumnsMYSQL += columnInformation + ", ";
                    }
                    #endregion

                    #region XMLQuery Sütunları generate edilir
                    entityColumnsXML += new CSHARP_To_MYSQL().GetXMLDataFormat(entityColumn);
                    #endregion
                }

                if (!string.IsNullOrEmpty(entityColumnsMYSQL))
                {
                    entityColumnsMYSQL = entityColumnsMYSQL.Remove(entityColumnsMYSQL.Length - 2);
                    createTableMYSQLQuery += $"CREATE TABLE {entityInformation.TableName} ({entityColumnsMYSQL});";
                }

                // Veritabanında versiyonlama için kullanılacak XML bilgisinin gövdesi
                createXMLObjectQuery += $"<{entityType.Name} TableName=\"{entityInformation.TableName}\" OrdinalPosition=\"{entityInformation.OrdinalPosition}\">" +
                                        $"{entityColumnsXML}" +
                                        $"</{entityType.Name}>";

                #region Tablo tableList listesinden çıkartılır
                DBTableModel tableInformation = tableList.FirstOrDefault(x =>x.TableName == entityInformation.TableName.ToLower());
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
                        Tuple<string,string> constraintTupleData = new DB_Operation(contextName).ConstraintNameByTableAndColumnName(entityInformation.TableName, columnName);
                        if (!string.IsNullOrEmpty(constraintTupleData.Item1))
                        {
                            if (constraintTupleData.Item2 == "PRIMARY KEY")
                            {
                                dropConstraintList.Add($"ALTER TABLE {entityInformation.TableName} DROP PRIMARY KEY;");

                                // MYSQL Auto Increment olan bir sütunu DROP edemiyor
                                // Önce Sütundan Auto Increment kaldırılır daha sonra PRIMARY KEY DROP edilir
                                string columnInformation = new DB_Operation(contextName).ColumnInformationFromDB(entityInformation.TableName, columnName);
                                dropConstraintList.Add($"ALTER TABLE {entityInformation.TableName} MODIFY COLUMN {columnName} {columnInformation};");
                            }
                            if (constraintTupleData.Item2 == "FOREIGN KEY")
                                dropConstraintList.Add($"ALTER TABLE {entityInformation.TableName} DROP FOREIGN KEY {constraintTupleData.Item1};");
                        }
                        columnNames += $"DROP {columnName}, ";
                    }
                    columnNames = columnNames.Remove(columnNames.Length - 2);
                    alterTableMYSQLQuery += $"ALTER TABLE {entityInformation.TableName} {columnNames};";
                }
                #endregion

                entityColumnsMYSQL = string.Empty;
                entityColumnsXML = string.Empty;
            } // Entity Class yani Veritabaındaki Tabloları oluşturmak için döngü sonlandırılır
            createXMLObjectQuery += "</Classes>"; // Veritabanında versiyonlama için kullanılacak XML bilgisinin bitişi
            #endregion

            #region DROP edilecek tablolar
            if (tableList.Count() > 0)
            {
                #region __WORM__Configuration dosyası silenecek tablolar listesinden kaldırılır
                DBTableModel wormTable = tableList.FirstOrDefault(x => x.TableName == "__worm__configuration");
                if (wormTable != null)
                    tableList.Remove(wormTable);
                #endregion

                // Tablo listesinde kalan herhangi bir kayıt Veritabanından silinecek tabloyu işaret eder.
                // Yani Kod tarafında bu tabloya ait Entity Class silinmiştir.
                foreach (DBTableModel table in tableList)
                {
                    dropTableMYSQLQuery += $"DROP TABLE {table.TableName};";
                }
            }
            #endregion

            #region Constraintler kaldırılır
            dropConstraintList.Reverse();
            foreach (string item in dropConstraintList)
            {
                dropConstraintListMYSQLQuery += item;
            }
            #endregion

            #endregion

            //                                                    DROP TABLES          CREATE TABLES             ALTER TABLES           XML QUERY
            return Tuple.Create(dropConstraintListMYSQLQuery +dropTableMYSQLQuery + createTableMYSQLQuery +  alterTableMYSQLQuery, createXMLObjectQuery);
        }
    }
}
