﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.ORACLE.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class VARCHAR2 : BaseAttribute
    {
        public VARCHAR2(int maxlength = 4000) : base("Type", "VARCHAR2")
        {
            this.maxlength = maxlength;
        }
        private int maxlength;
        public int MaxLength
        {
            get { return maxlength; }
            set { maxlength = value; }
        }
        public override object TypeId => 1000;

    }
}
