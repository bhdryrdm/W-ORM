using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;
using W_ORM.MSSQL.Attributes;

namespace W_ORM.MSSQL
{
    public class ContextGenerate
    {
        /// <summary>
        /// Define the compile unit to use for code generation. 
        /// </summary>
        CodeCompileUnit targetUnit;

        /// <summary>
        /// The only class in the compile unit. This class contains 2 fields,
        /// 3 properties, a constructor, an entry point, and 1 simple method. 
        /// </summary>
        CodeTypeDeclaration targetClass;


        /// <summary>
        /// Define the class.
        /// </summary>
        public void CreateContextEntity(string entityName, string contextName)
        {
            targetUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace(contextName);
            samples.Imports.Add(new CodeNamespaceImport("W_ORM.Layout.Attributes"));
            samples.Imports.Add(new CodeNamespaceImport("W_ORM.MSSQL.Attributes"));
            targetClass = new CodeTypeDeclaration(entityName);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            samples.Types.Add(targetClass);
            targetUnit.Namespaces.Add(samples);
        }


        /// <summary>
        /// Add three properties to the class.
        /// </summary>
        public void AddProperties(string entityColumnName,Type entityColumnType,List<string> customAttributes)
        {
            // Declare the read-only Width property.

            CodeMemberProperty property = new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = entityColumnName,
                Type = new CodeTypeReference(entityColumnType),
            };

            foreach (string customAttribute in customAttributes)
            {
                property.CustomAttributes.Add(new CodeAttributeDeclaration(customAttribute));
            }
            
            targetClass.Members.Add(property);
        }

        /// <summary>
        /// Generate CSharp source code from the compile unit.
        /// </summary>
        /// <param name="filename">Output file name</param>
        public void GenerateCSharpCode(string path,string entityName)
        {

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions { BracingStyle = "C" };
 
            if (!Directory.Exists(path))
            {
                DirectoryInfo directory = Directory.CreateDirectory(path);
                directory.Attributes = FileAttributes.Directory | FileAttributes.Normal;
            }
            
            using (StreamWriter sourceWriter = new StreamWriter(path + entityName))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }
        }
    }
}
