using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using W_ORM.Layout.DBConnection;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;

namespace W_ORM.MSSQL
{
    public class DB_Operation :  IDB_Operation , IDB_Operation_Helper , IDB_Generator
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
                    SqlCommand command = new SqlCommand(CreateDatabaseQuery(), (SqlConnection)connection);
                    command.ExecuteNonQuery();
                    #endregion

                    #region Oluşturulan veritabanına geçiş yapılır
                    connection.ChangeDatabase(this.contextName);
                    #endregion

                    #region __WORM__Configuration Tablosu var olup olmadığı kontrol edilerek oluşturulur
                    Tuple<string, SqlCommand> worm_Table = Create__WORM__Configuration_Table(tablesXMLForm);
                    command = new SqlCommand(worm_Table.Item1, (SqlConnection)connection);
                    command.ExecuteNonQuery();

                    command = worm_Table.Item2;
                    command.ExecuteNonQuery();
                    #endregion

                    #region Tablolar oluşturulur
                    command = new SqlCommand(createTableSQLQuery, (SqlConnection)connection);
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
            return $"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = N'{this.contextName}') BEGIN CREATE DATABASE {this.contextName} END";
        }

        private Tuple<string, SqlCommand> Create__WORM__Configuration_Table(string tablesXMLForm)
        {
            string configurationTableQuery = $"IF  NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'__WORM__Configuration')" +
                                                        "BEGIN " +
                                                        "CREATE TABLE [dbo].[__WORM__Configuration]( " +
                                                        "Version int IDENTITY(1,1) PRIMARY KEY," +
                                                        "UpdatedTime datetime," +
                                                        "UpdatedAuthor nvarchar(200)," +
                                                        "TablesXMLForm nvarchar(max)" +
                                                        ") END";

            DBInformationModel dBInformationModel = DBConnectionFactory.ReturnDBInformatinFromXML(this.ContextName);
            SqlCommand command = new SqlCommand($"INSERT INTO [dbo].[__WORM__Configuration](UpdatedTime, UpdatedAuthor, TablesXMLForm) " +
                                     $"VALUES(@UpdatedTime,@UpdatedAuthor,@TablesXMLForm)",
                                    (SqlConnection)connection);
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
                    SqlCommand command = new SqlCommand($"SELECT CASE WHEN EXISTS((SELECT NAME FROM MASTER.dbo.SYSDATABASES WHERE NAME = @DbName)) THEN 1 ELSE 0 END",
                                                        (SqlConnection)connection);
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
                using (connection = DBConnectionFactory.Instance(this.contextName))
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
                using (connection = DBConnectionFactory.Instance(this.ContextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    SqlCommand command = new SqlCommand($"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName", (SqlConnection)connection);
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

        public bool IsTableHasPrimaryKey(string schemaName,string tableName)
        {
            bool tablehasPrimaryKey = false;
            try
            {
                using (connection = DBConnectionFactory.Instance(this.contextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    SqlCommand command = new SqlCommand($"SELECT CASE WHEN EXISTS((SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE  TABLE_NAME = @TableName "+
                                                        "AND TABLE_SCHEMA = @SchemaName" +
                                                        "AND CONSTRAINT_TYPE = 'PRIMARY KEY')) THEN 1 ELSE 0 END",
                                                        (SqlConnection)connection);
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
                using (connection = DBConnectionFactory.Instance(this.ContextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    SqlCommand command = new SqlCommand($"SELECT * FROM [dbo].[__WORM__Configuration] WHERE Version=@Version", (SqlConnection)connection);
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
                                    contextGenerate.AddProperties(pocoColumn.Name, new MSSQL_To_CSHARP().XML_To_CSHARP(pocoColumn.Attributes.GetNamedItem("type").Value), customAttributes);
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
        #endregion

    }
}
