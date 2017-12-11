using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using W_ORM.Layout.DBType;

namespace W_ORM.MSSQL
{
    public static class WORM_Config_Creator
    {
        /// <summary>
        /// WORM.config dosyasına yeni context veritabanı eklemek için kullanılır
        /// </summary>
        /// <typeparam name="TContext">Context Veritabanı Adı</typeparam>
        /// <param name="connectionString">Context(Veritabanı) bağlantı cümlesi</param>
        /// <param name="dBType_Enum">Veritabanı tipi</param>
        /// <param name="author">Yazar ismi</param>
        public static void SaveWormConfig<TContext>(string path, string connectionString, DBType_Enum dBType_Enum, string author)
        {
            #region WORM.config dosyası var mı kontrolü yapılır
            path += "\\WORM.config";
            if (!File.Exists(path))
            {
                using (File.Create(path)) { };
                new XDocument(new XElement("Databases")).Save(path);

            }
            #endregion

            #region WORM.config dosyasından tüm contextler okunur
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            List<string> allContext = new List<string>();
            foreach (XmlNode item in xmlDocument.GetElementsByTagName("Databases"))
            {
                foreach (XmlNode context in item.ChildNodes)
                {
                    allContext.Add(context.Name);
                }
            }
            #endregion

            #region WORM.config dosyasına yeni context oluşturulur
            string contextName = typeof(TContext).Name;
            if (allContext.FirstOrDefault(x => x == contextName) == null)
            {
                XDocument xDocument = XDocument.Load(path);
                XElement contextRoot = new XElement(contextName);
                contextRoot.Add(new XAttribute("id", contextName));
                contextRoot.Add(new XElement("ConnectionString", new XAttribute("value", connectionString)),
                                new XElement("Provider", new XAttribute("value", GetProviderFactoryByEnum(dBType_Enum))),
                                new XElement("UpdatedAuthor", new XAttribute("value", author)));


                xDocument.Element("Databases").Add(contextRoot);
                xDocument.Save(path);
            }
            else
            {
                throw new Exception("Eklenmek istenen " + contextName + " contexti WORM.Config dosyasında zaten bulunmaktadır.Lütfen farklı bir context ismi belirleyiniz!");
            }
            #endregion
        }

        public static string GetProviderFactoryByEnum(DBType_Enum dBType_Enum)
        {
            string returnValue = string.Empty;
            switch (dBType_Enum)
            {
                case DBType_Enum.MSSQL:
                    returnValue = "System.Data.SqlClient"; break;
                case DBType_Enum.MYSQL:
                    returnValue = "MySql.Data.MySqlClient"; break;
                default: break;
            }
            return returnValue;
        }
    }
}
