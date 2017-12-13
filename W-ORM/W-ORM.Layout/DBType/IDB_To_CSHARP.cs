using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBType
{
    /// <summary>
    /// TR : XML verisinden CSHARP tipine çevirmek için kullanılır
    /// EN : 
    /// </summary>
    public interface IDB_To_CSHARP
    {
       Type XML_To_CSHARP(string xmlType);
    }
}
