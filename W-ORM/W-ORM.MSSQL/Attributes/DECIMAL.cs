﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.MSSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class DECIMAL : BaseAttribute
    {
        private int _stair;
        private int _comma;

        public DECIMAL(int stair, int comma) : base("Type", "DECIMAL")
        {
            this._stair = stair;
            this._comma = comma;
        }

        public int Stair
        {
            get{return _stair;}
            set{_stair = value;}
        }
        public int Comma
        {
            get { return _comma; }
            set { _comma = value; }
        }
        public override object TypeId => 1000;
    }

}
