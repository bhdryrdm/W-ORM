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
        public BaseAttribute(string defination, string name)
        {
            this.AttributeDefination = defination;
            this.AttributeName = name;
        }
        public string AttributeDefination { get; set; }
        public string AttributeName { get; set; }

    }
}
