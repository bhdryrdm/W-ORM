using System;
using System.Reflection;

namespace W_ORM.Layout.DBType
{
    /// <summary>
    /// TR : CSHARP yazım şeklinden gerekli olan SQL yazım şekline dönüştüren methodlar
    /// EN : 
    /// </summary>
    public interface ICSHARP_To_DB
    {
        /// <summary>
        /// TR : 
        /// EN : 
        /// </summary>
        /// <param name="propertyInfo">TR : Entity Class(POCO) içerisinde bulunan property EN : </param>
        /// <returns></returns>
        string GetSQLQueryFormat(PropertyInfo propertyInfo);

        /// <summary>
        /// TR : 
        /// EN : 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        string GetXMLDataFormat(PropertyInfo propertyInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        string Attribute_To_SQLType(Attribute attribute);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string PropertyType_To_SQLType(string propertyName);
    }
}
