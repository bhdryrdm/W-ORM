using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public abstract class BaseAttribute : Attribute
    {
        public BaseAttribute(string defination)
        {
            this.AttributeDefination = defination;
        }
        public string AttributeDefination { get; set; }
    }
}
