using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBProvider
{
    /// <summary>
    /// TR : Veritabanında bulunan __WORM__Configuration tablosu üzerinden seçilen versiyon ile Context generate etmek için kullanılır
    /// EN : 
    /// </summary>
    public interface IDB_Generator
    {
        /// <summary>
        /// TR : Veritabanı üzerinden seçilen versiyon numarası ile birlikte verilen parametreler doğrultusunda Context generate edilir
        /// EN : 
        /// </summary>
        /// <param name="dbVersion">TR : Generate edilecek Versiyon Numarası EN : </param>
        /// <param name="contextPath">TR : Generate edilecek Dosya Yolu EN : </param>
        /// <param name="namespaceName">TR : Generate edilecek Namespace EN : </param>
        /// <param name="contextName">TR : Generate edilecek Context Adı EN : </param>
        /// <returns></returns>
        bool ContextGenerateFromDB(int dbVersion, string contextPath = "", string namespaceName = "");
    }
}
