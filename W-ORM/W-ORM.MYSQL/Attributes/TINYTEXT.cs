﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.MYSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TINYTEXT : BaseAttribute
    {
        public TINYTEXT() : base("Type", "TINYTEXT")
        {
        }
        public override object TypeId => 1000;
    }
}
