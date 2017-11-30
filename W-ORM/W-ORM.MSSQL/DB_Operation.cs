using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBConnection;
using W_ORM.Layout.DBModel;
using W_ORM.Layout.DBProvider;

namespace W_ORM.MSSQL
{
    public class DB_Operation :  IDB_Operation 
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

        public bool CreateDatabase()
        {
            bool dbCreatedSuccess = true;
            try
            {
                using (connection = DBConnectionFactory.CreateDatabaseInstance(this.contextName))
                {

                    DBConnectionOperation.ConnectionOpen(connection);
                    command = connection.CreateCommand();
                    command.CommandText = $"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = N'{this.contextName}')" +
                                          "BEGIN " +
                                          $"CREATE DATABASE {this.contextName} " +
                                          "END";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                dbCreatedSuccess = false;
            }
            DBConnectionOperation.ConnectionClose(connection);
            return dbCreatedSuccess;
        }

        public bool CreateSettingTable(string tablesXMLForm)
        {
            bool dbTabledSuccess = true;
            try
            {
                using (connection = DBConnectionFactory.Instance(this.ContextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    SqlCommand command = new SqlCommand($"IF  NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'__WORM__Configuration')" +
                                                          "BEGIN " +
                                                          "CREATE TABLE [dbo].[__WORM__Configuration]( " +
                                                          "Version int IDENTITY(1,1) PRIMARY KEY," +
                                                          "UpdatedTime datetime," +
                                                          "UpdatedAuthor nvarchar(200)," +
                                                          "TablesXMLForm nvarchar(max)" +
                                                          ") END", (SqlConnection)connection);
                    command.ExecuteNonQuery();


                    DBInformationModel dBInformationModel = DBConnectionFactory.ReturnDBInformatinFromXML(this.ContextName);
                    command = new SqlCommand($"INSERT INTO [dbo].[__WORM__Configuration](UpdatedTime, UpdatedAuthor, TablesXMLForm) " +
                                             $"VALUES(@UpdatedTime,@UpdatedAuthor,@TablesXMLForm)",
                                            (SqlConnection)connection);
                    command.Parameters.AddWithValue("@UpdatedTime", dBInformationModel.UpdatedTime);
                    command.Parameters.AddWithValue("@UpdatedAuthor", dBInformationModel.UpdatedAuthor);
                    command.Parameters.AddWithValue("@TablesXMLForm", tablesXMLForm);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                dbTabledSuccess = false;
            }

            DBConnectionOperation.ConnectionClose(connection);
            return dbTabledSuccess;
        }

        public bool CreateTable(string createTableSQLQuery)
        {
            bool dbTabledSuccess = true;
            try
            {
                using (connection = DBConnectionFactory.Instance(this.contextName))
                {
                    DBConnectionOperation.ConnectionOpen(connection);
                    command = connection.CreateCommand();
                    command.CommandText = createTableSQLQuery;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                dbTabledSuccess = false;
            }
            DBConnectionOperation.ConnectionClose(connection);
            return dbTabledSuccess;
        }

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
                    command.Parameters.AddWithValue("@Version",dbVersion);
                
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


        public List<DBTableModel> TableListOnDB()
        {
            List<DBTableModel> tableList = new List<DBTableModel>();
            try
            {
                using (connection = DBConnectionFactory.Instance(this.ContextName))
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
            }
            DBConnectionOperation.ConnectionClose(connection);
            return tableList;
        }

        /// <summary>
        /// Tablodaki tüm sütunları getirir
        /// </summary>
        /// <param name="tableName">Veritabanı üzerindeki tablo ismi</param>
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
            catch (Exception)
            { 
            }
            
            DBConnectionOperation.ConnectionClose(connection);
            return columnList;
        }

    }
}
