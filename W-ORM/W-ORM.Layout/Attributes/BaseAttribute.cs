using System;

namespace W_ORM.Layout.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public abstract class BaseAttribute : Attribute
    {
        /// <summary>
        /// TR : Veritabanı versiyonlaması için XML versisine eklenecek olan Attribute tanımı(Type,Null,PKey,FKey vb.) 
        /// EN : 
        /// </summary>
        /// <param name="defination">Attribute Tanımı(Type,Null,PKey,FKey vb.)</param>
        /// <param name="name">Attribute Adı</param>
        public BaseAttribute(string defination, string name)
        {
            this.AttributeDefination = defination;
            this.AttributeName = name;
        }
        public string AttributeDefination { get; set; }
        public string AttributeName { get; set; }

    }
}
