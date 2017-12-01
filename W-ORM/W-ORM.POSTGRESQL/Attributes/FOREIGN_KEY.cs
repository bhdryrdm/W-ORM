using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.POSTGRESQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class FOREIGN_KEY : BaseAttribute
    {
        public FOREIGN_KEY(string className, string propertyName) : base("FKey", "FOREIGN_KEY")
        {
            this.className = className;
            this.propertyName = propertyName;
        }
        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private string propertyName;

        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        public override object TypeId => 4001;
    }
}
