using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace W_ORM.Layout.DBType
{
    public interface ICSHARP_To_DB
    {
        string GetSQLQueryFormat(PropertyInfo propertyInfo);
        string GetXMLDataFormat(PropertyInfo propertyInfo);
        string Attribute_To_SQLType(Attribute attribute);
        string PropertyType_To_SQLType(string propertyName);
    }
}
