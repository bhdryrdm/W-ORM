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
<<<<<<< HEAD
        private int _length;
        public INT(int length = 0) : base("Type")
        {
            this._length = length;
        }

        public int Length
        {
            get { return _length; }
            set { _length = value; }
=======
        public INT() : base("Type","INT")
        {

>>>>>>> f39a5d4865cbce928b573d39dbf3bd842d2306be
        }
    }
}
