using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Xml;
using W_ORM.Layout.DBConnection;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;
using W_ORM.MYSQL.TypeConverter;

namespace W_ORM.MYSQL
{
    public class DB_Operation : IDB_Operation, IDB_Operation_Helper, IDB_Generator
    {
        string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        string projectNamespace = Assembly.GetCallingAssembly().FullName.Split(',')[0].Replace("-", "_");
        DbConnection connection = null;

        #region Property & Constructor

        private string contextName;

        public string ContextName
        {
            get { return contextName; }
            set { contextName = value; }
        }

        public DB_Operation(string contextName)
        {
            this.contextName = contextName;
        }
        #endregion

        #region IDB_Operation
        public bool CreateORAlterDatabaseAndTables(string tablesXMLForm, string createTableSQLQuery)
        {
            bool dbCreatedSuccess = true;
            try
            {
                using (connection = DBConnectionFactory.CreateDatabaseInstance(this.contextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);

                    #region Veritabanı var olup olmadığı konrtrol edilerek oluşturulur
                    MySqlCommand command = new MySqlCommand(CreateDatabaseQuery(), (MySqlConnection)connection);
                    command.ExecuteNonQuery();
                    #endregion

                    #region Oluşturulan veritabanına geçiş yapılır
                    connection.ChangeDatabase(this.contextName);
                    #endregion

                    #region __WORM__Configuration Tablosu var olup olmadığı kontrol edilerek oluşturulur
                    Tuple<string, MySqlCommand> worm_Table = Create__WORM__Configuration_Table(tablesXMLForm);
                    command = new MySqlCommand(worm_Table.Item1, (MySqlConnection)connection);
                    command.ExecuteNonQuery();

                    command = worm_Table.Item2;
                    command.ExecuteNonQuery();
                    #endregion

                    #region Tablolar oluşturulur
                    command = new MySqlCommand(createTableSQLQuery, (MySqlConnection)connection);
                    command.ExecuteNonQuery();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                dbCreatedSuccess = false;
                throw ex;
            }
            return dbCreatedSuccess;
        }

        private string CreateDatabaseQuery()
        {
            return $"CREATE DATABASE IF NOT EXISTS `{ this.contextName.ToLower()}`";
        }

        private Tuple<string, MySqlCommand> Create__WORM__Configuration_Table(string tablesXMLForm)
        {
            string configurationTableQuery = $"CREATE TABLE IF NOT EXISTS __worm__configuration ( " +
                                                "Version int NOT NULL AUTO_INCREMENT," +
                                                "UpdatedTime DATETIME," +
                                                "UpdatedAuthor VARCHAR(255)," +
                                                "TablesXMLForm MEDIUMTEXT," +
                                                "PRIMARY KEY (Version) " +
                                                ");";

            DBInformationModel dBInformationModel = DBConnectionFactory.ReturnDBInformatinFromXML(this.ContextName);
            MySqlCommand command = new MySqlCommand($"INSERT INTO __WORM__Configuration(UpdatedTime, UpdatedAuthor, TablesXMLForm) " +
                                     $"VALUES(@UpdatedTime,@UpdatedAuthor,@TablesXMLForm)",
                                    (MySqlConnection)connection);
            command.Parameters.AddWithValue("@UpdatedTime", DateTime.Now);
            command.Parameters.AddWithValue("@UpdatedAuthor", dBInformationModel.UpdatedAuthor);
            command.Parameters.AddWithValue("@TablesXMLForm", tablesXMLForm);

            return Tuple.Create(configurationTableQuery, command);
        }

        #endregion

        #region IDB_Operation_Helper
        /// <summary>
        /// Veritabanın var olup olmadığı kontrolünü sağlar
        /// </summary>
        /// <returns></returns>
        public bool DatabaseExistControl()
        {
            bool dbExist = false;
            try
            {
                using (connection = DBConnectionFactory.Instance(this.contextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    MySqlCommand command = new MySqlCommand($"SELECT CASE WHEN EXISTS((SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @DbName)) THEN 1 ELSE 0 END",
                                                        (MySqlConnection)connection);
                    command.Parameters.AddWithValue("@DbName", this.contextName);
                    dbExist = (int)command.ExecuteScalar() == 1 ? true : false;
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                throw ex;
            }
            return dbExist;
        }

        /// <summary>
        /// Veritabanı üzerindeki tabloları isim ve şema birlikte listeler
        /// </summary>
        /// <returns></returns>
        public List<DBTableModel> TableListOnDB()
        {
            List<DBTableModel> tableList = new List<DBTableModel>();
            try
            {
                using (connection = DBConnectionFactory.Instance(this.contextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    DataTable tables = connection.GetSchema("Tables");
                    foreach (DataRow table in tables.Rows)
                    {
                        DBTableModel dBTableModel = new DBTableModel
                        {
                            TableName = table[2].ToString()

                        };
                        tableList.Add(dBTableModel);
                    }
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
            }
            return tableList;
        }

        /// <summary>
        /// Tablo üzerindeki sütunların isimlerini listeler
        /// </summary>
        /// <param name="tableName">Sütunları listelenecek tablo adı</param>
        /// <returns></returns>
        public List<string> ColumnListOnTable(string tableName)
        {
            List<string> columnList = new List<string>();
            try
            {
                using (connection = DBConnectionFactory.Instance(this.ContextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    MySqlCommand command = new MySqlCommand($"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName", (MySqlConnection)connection);
                    command.Parameters.AddWithValue("@TableName", tableName);

                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        columnList.Add(reader.GetString(3));
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
            }
            return columnList;
        }

        public bool IsTableHasPrimaryKey(string schemaName, string tableName)
        {
            bool tablehasPrimaryKey = false;
            try
            {
                using (connection = DBConnectionFactory.Instance(this.contextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    MySqlCommand command = new MySqlCommand($"SELECT CASE WHEN EXISTS((SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE  TABLE_NAME = @TableName " +
                                                        "AND TABLE_SCHEMA = @SchemaName" +
                                                        "AND CONSTRAINT_TYPE = 'PRIMARY KEY')) THEN 1 ELSE 0 END",
                                                        (MySqlConnection)connection);
                    command.Parameters.AddWithValue("@SchemaName", schemaName);
                    command.Parameters.AddWithValue("@TableName", tableName);
                    tablehasPrimaryKey = (int)command.ExecuteScalar() == 1 ? true : false;
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                throw ex;
            }
            return tablehasPrimaryKey;
        }

        // <summary>
        /// Tablo ve Sütun adı verilerek ilgili Constraint ismini döner
        /// </summary>
        /// <param name="schemaName">Tablo Şema Adı</param>
        /// <param name="tableName">Tablo Adı</param>
        /// <param name="columnName">Sütun Adı</param>
        /// <returns></returns>
        public Tuple<string,string> ConstraintNameByTableAndColumnName(string tableName, string columnName, string schemaName = "")
        {
            string constraintName = string.Empty;
            string constraintType = string.Empty;
            try
            {
                using (connection = DBConnectionFactory.Instance(this.ContextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    MySqlCommand command = new MySqlCommand($"SELECT Col.COLUMN_NAME,Col.CONSTRAINT_NAME,Tbl.CONSTRAINT_TYPE " +
                                                            $"FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tbl,INFORMATION_SCHEMA.KEY_COLUMN_USAGE Col " +
                                                            $"WHERE Col.CONSTRAINT_NAME = Tbl.CONSTRAINT_NAME " +
                                                            $"AND Col.TABLE_NAME = Tbl.TABLE_NAME " +
                                                            $"AND Col.TABLE_NAME = @TableName " +
                                                            $"AND Col.COLUMN_NAME = @ColumnName",(MySqlConnection)connection);
                    command.Parameters.AddWithValue("@TableName", tableName);
                    command.Parameters.AddWithValue("@ColumnName", columnName);

                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        constraintName = reader.GetString(1);
                        constraintType = reader.GetString(2);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                throw ex;
            }
            return Tuple.Create(constraintName, constraintType);
        }

        public int LatestVersionDatabase()
        {
            int version = 0;
            try
            {
                using (connection = DBConnectionFactory.Instance(this.ContextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    MySqlCommand command = new MySqlCommand($"SELECT Version FROM __WORM__Configuration ORDER BY Version DESC LIMIT 1", (MySqlConnection)connection);

                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        version = reader.GetInt32(0);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                throw ex;
            }
            return version;
        }

        #endregion

        #region IDB_Generator
        public bool ContextGenerateFromDB(int dbVersion, string contextPath = "", string namespaceName = "", string contextName = "")
        {

            if (string.IsNullOrEmpty(contextPath))
                contextPath = projectPath + "\\WORM_Context\\";
            if (string.IsNullOrEmpty(namespaceName))
                namespaceName = projectNamespace + ".WORM_Context";
            if (string.IsNullOrEmpty(contextName))
                contextName = this.contextName;

            bool dbCreatedSuccess = true;
            List<string> POCOClasses = new List<string>();
            try
            {
                using (connection = DBConnectionFactory.Instance(this.ContextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    MySqlCommand command = new MySqlCommand($"SELECT * FROM __WORM__Configuration WHERE Version=@Version", (MySqlConnection)connection);
                    command.Parameters.AddWithValue("@Version", dbVersion);

                    DbDataReader reader = command.ExecuteReader();

                    #region XML verisinden POCO Entity Class ları oluşturulur
                    while (reader.Read())
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(reader.GetString(3));
                        XmlNodeList xmlTableForm = xmlDoc.GetElementsByTagName("Classes");

                        ContextGenerate POCOClassGenerate = new ContextGenerate();
                        foreach (XmlNode pocoClasses in xmlTableForm)
                        {
                            foreach (XmlNode pocoClass in pocoClasses.ChildNodes)
                            {
                                List<string> pocoClasscustomAttributes = new List<string>();
                                foreach (XmlAttribute pocoClassAttribute in pocoClass.Attributes)
                                {
                                    pocoClasscustomAttributes.Add(pocoClassAttribute.Value);
                                }
                                POCOClassGenerate.CreateContextEntity(pocoClass.Name, namespaceName + ".Entities", pocoClasscustomAttributes);
                                POCOClasses.Add(pocoClass.Name);
                                foreach (XmlNode pocoColumn in pocoClass.ChildNodes)
                                {
                                    Dictionary<string, string> pocoColumncustomAttributes = new Dictionary<string, string>();
                                    foreach (XmlAttribute pocoColumnAttribute in pocoColumn.Attributes)
                                    {
                                        pocoColumncustomAttributes.Add(pocoColumnAttribute.Name, pocoColumnAttribute.Value);
                                    }
                                    POCOClassGenerate.AddProperties(pocoColumn.Name, new MYSQL_To_CSHARP().XML_To_CSHARP(pocoColumn.Attributes.GetNamedItem("Type").Value), pocoColumncustomAttributes);
                                }
                                POCOClassGenerate.GenerateCSharpCode(contextPath + "Entities\\", $"{pocoClass.Name}.cs");
                            }
                        }
                    }
                    reader.Close();
                    #endregion

                    #region XML verisinden Context Class oluşturulur
                    // TODO : Otomatik oluşturulan Entity Class lar exclude halinde geliyor
                    ContextGenerate contextGenerate = new ContextGenerate();
                    contextGenerate.CreateContextEntity(contextName, namespaceName);
                    //foreach (string pocoClass in POCOClasses)
                    //{
                    //    contextGenerate.AddProperties(pocoClass, "MYSQLProviderContext", contextName);
                    //}
                    contextGenerate.GenerateCSharpCode(contextPath, $"{contextName}.cs");
                    #endregion
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                dbCreatedSuccess = false;
                throw ex;
            }
            return dbCreatedSuccess;
        }
        #endregion

        #region Custom_MYSQL_Helper
        public string ColumnInformationFromDB(string tableName, string columnName)
        {
            string columnInformation = string.Empty;
            try
            {
                using (connection = DBConnectionFactory.Instance(this.ContextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    MySqlCommand command = new MySqlCommand($"SELECT DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS  " +
                                                            $"WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName", (MySqlConnection)connection);
                    command.Parameters.AddWithValue("@TableName", tableName);
                    command.Parameters.AddWithValue("@ColumnName", columnName);

                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        columnInformation = $"{reader.GetString(0)}{(reader.IsDBNull(1) ? "" : $"({reader.GetString(1)})")} {(reader.GetString(2) == "YES" ? "NULL" : "NOT NULL")} "; 
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                throw ex;
            }
            return columnInformation;
        }
        #endregion
    }
}
