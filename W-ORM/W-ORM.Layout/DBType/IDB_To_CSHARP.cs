﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBType
{
    public interface IDB_To_CSHARP
    {
       Type XML_To_CSHARP(string xmlType);
    }
}