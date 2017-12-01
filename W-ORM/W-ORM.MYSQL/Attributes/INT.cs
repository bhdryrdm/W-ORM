using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.MYSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class INT : BaseAttribute
    {
        public INT(int length = 0) : base("Type", "INT")
        {
            this._length = length;
        }

        public int Length
        {
            get { return _length; }
            set { _length = value; }

        }
        private int _length;

        public override object TypeId => 1000;
    }
}
