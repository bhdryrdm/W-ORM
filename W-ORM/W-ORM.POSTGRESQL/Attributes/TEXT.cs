﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.POSTGRESQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TEXT : BaseAttribute
    {
        public TEXT() : base("Type", "TEXT")
        {
        }
        public override object TypeId => 1000;

    }
}
