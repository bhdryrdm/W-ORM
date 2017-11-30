using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.ContextProvider
{
    public interface IContextProvider
    {
        bool GenerateContextFromDB(int versionNumber,string contextFolderName);
    }
}
