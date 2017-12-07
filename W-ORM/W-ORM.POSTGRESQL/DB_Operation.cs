using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml;
using W_ORM.Layout.DBConnection;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;
using W_ORM.POSTGRESQL.TypeConverter;

namespace W_ORM.POSTGRESQL
{
    public class DB_Operation : IDB_Operation, IDB_Operation_Helper, IDB_Generator
    {
        DbCommand command = null;
        string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        DbConnection connection = null;

        #region Property & Constructor

        private string contextName;

        public string ContextName
        {
            get { return contextName; }
            set { contextName = value; }
        }
        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
        }
        private string connectionStringWithDB;

        public string ConnectionStringWithDB
        {
            get { return connectionStringWithDB; }
        }

        public DB_Operation(string contextName)
        {
            this.contextName = contextName;
            connectionString = DBConnectionFactory.ReturnDBInformatinFromXML(contextName).ConnectionString;
            connectionStringWithDB = connectionString + $"Database={contextName};";
        }
        #endregion

        #region IDB_Operation
        public bool CreateORAlterDatabaseAndTables(string tablesXMLForm, string createTableSQLQuery)
        {
            bool dbCreatedSuccess = true;
            try
            {
                using (connection = new NpgsqlConnection(this.connectionString))
                {
                    DBConnectionOperation.ConnectionOpen(connection);

                    #region Veritabanı var olup olmadığı konrtrol edilerek oluşturulur
                    NpgsqlCommand command = new NpgsqlCommand(CreateDatabaseQuery(), (NpgsqlConnection)connection);
                    command.ExecuteNonQuery();
                    #endregion

                    #region Oluşturulan veritabanına geçiş yapılır
                    connection.ChangeDatabase(this.contextName);
                    #endregion

                    #region __WORM__Configuration Tablosu var olup olmadığı kontrol edilerek oluşturulur
                    Tuple<string, NpgsqlCommand> worm_Table = Create__WORM__Configuration_Table(tablesXMLForm);
                    command = new NpgsqlCommand(worm_Table.Item1, (NpgsqlConnection)connection);
                    command.ExecuteNonQuery();

                    command = worm_Table.Item2;
                    command.ExecuteNonQuery();
                    #endregion

                    #region Tablolar oluşturulur
                    command = new NpgsqlCommand(createTableSQLQuery, (NpgsqlConnection)connection);
                    command.ExecuteNonQuery();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
                dbCreatedSuccess = false;
            }
            return dbCreatedSuccess;
        }

        private string CreateDatabaseQuery()
        {
            return $"CREATE DATABASE IF NOT EXISTS `{ this.contextName.ToLower()}`";
        }

        private Tuple<string, NpgsqlCommand> Create__WORM__Configuration_Table(string tablesXMLForm)
        {
            string configurationTableQuery = $"CREATE TABLE IF NOT EXISTS __worm__configuration ( " +
                                                "Version int NOT NULL AUTO_INCREMENT," +
                                                "UpdatedTime DATETIME," +
                                                "UpdatedAuthor VARCHAR(255)," +
                                                "TablesXMLForm MEDIUMTEXT," +
                                                "PRIMARY KEY (Version) " +
                                                ");";

            DBInformationModel dBInformationModel = DBConnectionFactory.ReturnDBInformatinFromXML(this.ContextName);
            NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO __WORM__Configuration(UpdatedTime, UpdatedAuthor, TablesXMLForm) " +
                                     $"VALUES(@UpdatedTime,@UpdatedAuthor,@TablesXMLForm)",
                                    (NpgsqlConnection)connection);
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
                using (connection = new NpgsqlConnection(this.connectionString))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    NpgsqlCommand command = new NpgsqlCommand($"SELECT CASE WHEN EXISTS((SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @DbName)) THEN 1 ELSE 0 END",
                                                        (NpgsqlConnection)connection);
                    command.Parameters.AddWithValue("@DbName", this.contextName);
                    dbExist = (int)command.ExecuteScalar() == 1 ? true : false;
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
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
                using (connection = new NpgsqlConnection(this.connectionString))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    DataTable tables = connection.GetSchema("Tables");
                    foreach (DataRow table in tables.Rows)
                    {
                        DBTableModel dBTableModel = new DBTableModel
                        {
                            SchemaName = table[1].ToString(),
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
                using (connection = new NpgsqlConnection(this.connectionString))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName", (NpgsqlConnection)connection);
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
                using (connection = new NpgsqlConnection(this.connectionString))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    NpgsqlCommand command = new NpgsqlCommand($"SELECT CASE WHEN EXISTS((SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE  TABLE_NAME = @TableName " +
                                                        "AND TABLE_SCHEMA = @SchemaName" +
                                                        "AND CONSTRAINT_TYPE = 'PRIMARY KEY')) THEN 1 ELSE 0 END",
                                                        (NpgsqlConnection)connection);
                    command.Parameters.AddWithValue("@SchemaName", schemaName);
                    command.Parameters.AddWithValue("@TableName", tableName);
                    tablehasPrimaryKey = (int)command.ExecuteScalar() == 1 ? true : false;
                }
            }
            catch (Exception ex)
            {
                DBConnectionOperation.ConnectionClose(connection);
            }
            return tablehasPrimaryKey;
        }
        #endregion

        #region IDB_Generator
        public bool ContextGenerateFromDB(int dbVersion, string contextPath = "", string contextName = "")
        {

            if (string.IsNullOrEmpty(contextPath))
                contextPath = projectPath + "\\WORM_Context\\";
            if (string.IsNullOrEmpty(contextName))
                contextName = "WORM_Context";

            bool dbCreatedSuccess = true;
            try
            {
                using (connection = new NpgsqlConnection(this.connectionString))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM [dbo].[__WORM__Configuration] WHERE Version=@Version", (NpgsqlConnection)connection);
                    command.Parameters.AddWithValue("@Version", dbVersion);

                    DbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(reader.GetString(3));
                        XmlNodeList xmlTableForm = xmlDoc.GetElementsByTagName("Classes");

                        ContextGenerate contextGenerate = new ContextGenerate();
                        foreach (XmlNode pocoClasses in xmlTableForm)
                        {
                            foreach (XmlNode pocoProperty in pocoClasses.ChildNodes)
                            {
                                contextGenerate.CreateContextEntity(pocoProperty.Name, contextName);
                                foreach (XmlNode pocoColumn in pocoProperty.ChildNodes)
                                {
                                    List<string> customAttributes = new List<string>();
                                    foreach (XmlAttribute pocoColumnAttribute in pocoColumn.Attributes)
                                    {
                                        customAttributes.Add(pocoColumnAttribute.Value);
                                    }
                                    contextGenerate.AddProperties(pocoColumn.Name, new POSTGRESQL_To_CSHARP().XML_To_CSHARP(pocoColumn.Attributes.GetNamedItem("type").Value), customAttributes);
                                }
                                contextGenerate.GenerateCSharpCode(contextPath, $"{pocoProperty.Name}.cs");
                            }
                        }
                    }
                    reader.Close();

                }
            }
            catch (Exception ex)
            {

                dbCreatedSuccess = false;
            }
            DBConnectionOperation.ConnectionClose(connection);
            return dbCreatedSuccess;
        }

        public string ConstraintNameByTableAndColumnName(string schemaName, string tableName, string columnName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}