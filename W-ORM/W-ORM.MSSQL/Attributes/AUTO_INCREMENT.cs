using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.MSSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class AUTO_INCREMENT : BaseAttribute
    {
        public AUTO_INCREMENT(int startNumber,int increase) : base("Increment","AUTO_INCREMENT")
        {
            this.startNumber = startNumber;
            this.increase = increase;
        }

        private int increase;

        public int Increase
        {
            get { return increase; }
            set { increase = value; }
        }


        private int startNumber;

        public int StartNumber
        {
            get { return startNumber; }
            set { startNumber = value; }
        }
        public override object TypeId => 3000;
    }
}
