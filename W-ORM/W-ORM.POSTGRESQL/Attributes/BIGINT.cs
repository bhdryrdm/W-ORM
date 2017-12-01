using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.POSTGRESQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class BIGINT : BaseAttribute
    {
        private int _length;
        public BIGINT(int length = 0) : base("Type", "BIGINT")
        {
            this._length = length;
        }
  
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
        public override object TypeId => 1000;

    }
}
