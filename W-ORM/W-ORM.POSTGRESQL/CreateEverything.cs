using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.Layout.DBModel;

namespace W_ORM.POSTGRESQL
{
    public class CreateEverything<TDBEntity> : POSTGRESQLProviderContext<TDBEntity>
    {
       

        public Tuple<string, string> GetPOSTGRESQLQueries()
        {
            Type entityType;
            dynamic entityInformation;
            string entityColumnsPOSTGRESQL = string.Empty, entityColumnPOSTGRESQLType = string.Empty, createTablePOSTGRESQLQuery = string.Empty,
                   entityColumnsXML = string.Empty, entityColumnXML = string.Empty, createXMLObjectQuery = string.Empty;
            List<dynamic> implementedEntities = (from property in EntityType.GetProperties()
                                                 from genericArguments in property.PropertyType.GetGenericArguments()
                                                 where genericArguments.CustomAttributes.FirstOrDefault().AttributeType.Equals(typeof(TableAttribute))
                                                 select Activator.CreateInstance(genericArguments)).ToList();
            createXMLObjectQuery = "<Classes>";
            foreach (var entity in implementedEntities)
            {
                entityType = entity.GetType();
                entityInformation = entityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();

                foreach (var entityColumn in entityType.GetProperties())
                {
                    entityColumnsPOSTGRESQL += new CSHARP_To_POSTGRESQL().GetSQLQueryFormat(entityColumn) + ", ";

                    entityColumnsXML += new CSHARP_To_POSTGRESQL().GetXMLDataFormat(entityColumn);
                }
                entityColumnsPOSTGRESQL = entityColumnsPOSTGRESQL.Remove(entityColumnsPOSTGRESQL.Length - 2);
                createTablePOSTGRESQLQuery += $"CREATE TABLE [{entityInformation.SchemaName}].[{entityInformation.TableName}] ({entityColumnsPOSTGRESQL}) ";


                createXMLObjectQuery += $"<{entityType.Name}>" +
                                    $"{entityColumnsXML}" +
                                    $"</{entityType.Name}>";

                entityColumnsPOSTGRESQL = string.Empty;
                entityColumnsXML = string.Empty;
            }
            createXMLObjectQuery += "</Classes>";

            return Tuple.Create(createTablePOSTGRESQLQuery, createXMLObjectQuery);
        }

    }
}
