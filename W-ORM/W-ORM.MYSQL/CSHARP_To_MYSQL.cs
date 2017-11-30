using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.DBType;

namespace W_ORM.MYSQL
{
    public class CSHARP_To_MYSQL : ICSHARP_To_DB
    {
        public string GetSQLQueryFormat(PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }

        public string GetXMLDataFormat(PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }

        public string Attribute_To_SQLType(Attribute attribute)
        {
            string response = string.Empty;
            switch (attribute.GetType().Name)
            {

            }
            return response;
        }
        public string PropertyName_To_SQLType(string propertyName)
        {
            throw new NotImplementedException();
        }

       
    }
}
