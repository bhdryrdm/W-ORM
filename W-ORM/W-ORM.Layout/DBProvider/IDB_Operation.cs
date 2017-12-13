namespace W_ORM.Layout.DBProvider
{
    /// <summary>
    /// TR : Veritabanı için Context Adı ve veritabanı oluşturma ve güncelleme için kullanılır
    /// EN : 
    /// </summary>
    public interface IDB_Operation
    {
        string ContextName { get; set; }
        /// <summary>
        /// TR : Veritabanı oluşturulurken ve güncellenirken çalıştırılacak fonksiyon
        /// EN : 
        /// </summary>
        /// <param name="tablesXMLForm">TR : Veritabanının XML formu  EN : </param>
        /// <param name="createTableSQLQuery">TR : Veritabanı oluşumu için gerekli olan SQL sorguları EN : </param>
        /// <returns></returns>
        bool CreateORAlterDatabaseAndTables(string tablesXMLForm, string createTableSQLQuery);
    }
}
