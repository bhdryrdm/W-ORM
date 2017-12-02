using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBModel;

namespace W_ORM.Layout.DBProvider
{
    public interface IDB_Operation_Helper
    {
        /// <summary>
        /// Veritabanı üzerindeki tabloları isim ve şema birlikte listeler
        /// </summary>
        /// <returns></returns>
        List<DBTableModel> TableListOnDB();

        /// <summary>
        /// Tablo üzerindeki sütunların isimlerini listeler
        /// </summary>
        /// <param name="tableName">Sütunları listelenecek tablo adı</param>
        /// <returns></returns>
        List<string> ColumnListOnTable(string tableName);

        /// <summary>
        /// Veritabanın var olup olmadığı kontrolünü sağlar
        /// </summary>
        /// <returns></returns>
        bool DatabaseExistControl();

        /// <summary>
        /// Tablonun Primary Key'e sahip olup olmadığını döner
        /// </summary>
        /// <param name="schemaName">Tablo Şema Adı</param>
        /// <param name="tableName">Tablo Adı</param>
        /// <returns></returns>
        bool IsTableHasPrimaryKey(string schemaName,string tableName);
    }
}
