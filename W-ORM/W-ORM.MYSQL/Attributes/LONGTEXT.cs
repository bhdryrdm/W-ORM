﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.MYSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class LONGTEXT : BaseAttribute
    {
        public LONGTEXT() : base("Type", "LONGTEXT")
        {
        }
        public override object TypeId => 1000;
    }
}
