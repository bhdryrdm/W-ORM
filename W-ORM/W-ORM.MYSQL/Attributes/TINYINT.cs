﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.MYSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TINYINT : BaseAttribute
    {
        private int _length;
        public TINYINT(int length = 0 ) : base("Type", "TINYINT")
        {
            this._length = length;
        }

        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
    }
}
