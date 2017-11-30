using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.MSSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class BINARY : BaseAttribute
    {
        private int _length;

        public BINARY(int length = 0) : base("Type")
        {
            this._length = length;
        }

        public int Length
        {
            get { return _length;}
            set{ _length = value;}
        }
    }
}
