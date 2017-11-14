using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Test
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SchemaAttribute : Attribute
    {
        private  string schemaName;

        public string SchemaName
        {
            get { return schemaName; }
            set { schemaName = value; }
        }

    }
}
