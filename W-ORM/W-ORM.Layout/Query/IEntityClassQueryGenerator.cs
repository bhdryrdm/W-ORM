using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBProvider;

namespace W_ORM.Layout.Query
{
    /// <summary>
    /// TR : Veritabanı oluşturmak için kullanılır
    /// EN : 
    /// </summary>
    /// <typeparam name="TDBEntity">TR : Context Adı (Veritabanı Adı) EN : Context Name (Database Name) </typeparam>
    public interface IEntityClassQueryGenerator<TDBEntity>
    {
        /// <summary>
        /// TR : SQL Server için oluşturulacak tabloların, sütunların sorguları generate edilir
        /// EN : 
        /// </summary>
        /// <returns></returns>
        Tuple<string, string> EntityClassQueries(); 
    }
}
