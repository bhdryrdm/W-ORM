using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
        public void CreateContextEntity(string entityName, string namespaceName, List<string> customAttributes = null)
        {
            targetUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace(namespaceName);
            samples.Imports.Add(new CodeNamespaceImport("W_ORM.Layout.Attributes"));
            samples.Imports.Add(new CodeNamespaceImport("W_ORM.MSSQL.Attributes"));
            targetClass = new CodeTypeDeclaration(entityName);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            samples.Types.Add(targetClass);
            if(customAttributes != null)
            {
                targetClass.CustomAttributes.Add(new CodeAttributeDeclaration
                {
                    Name = "Table",
                    Arguments =
                    {
                        new CodeAttributeArgument { Name = "SchemaName", Value = new CodePrimitiveExpression(customAttributes[0]) },
                        new CodeAttributeArgument { Name = "TableName", Value = new CodePrimitiveExpression(customAttributes[1]) }
                    }
                });
            }
            targetUnit.Namespaces.Add(samples);
        }

        public void CreateContextEntity(string contextName, string namespaceName)
        {
            targetUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace(namespaceName);
            samples.Imports.Add(new CodeNamespaceImport("W_ORM.Layout.DBModel"));
            samples.Imports.Add(new CodeNamespaceImport("W_ORM.MSSQL"));
            samples.Imports.Add(new CodeNamespaceImport(namespaceName + ".Entities"));
            targetClass = new CodeTypeDeclaration(contextName);
            targetClass.IsClass = true;
            targetClass.BaseTypes.Add(new CodeTypeReference { BaseType = $"BaseContext<{contextName}>"  });
            targetClass.TypeAttributes = TypeAttributes.Public;
            samples.Types.Add(targetClass);
            
            targetUnit.Namespaces.Add(samples);
        }

        /// <summary>
        /// Add three properties to the class.
        /// </summary>
        public void AddProperties(string entityColumnName,Type entityColumnType,List<string> customAttributes = null)
        {
            // Declare the read-only Width property.

            CodeMemberField property = new CodeMemberField
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = entityColumnName + " { get; set; } // "+ entityColumnName ,
                Type = new CodeTypeReference(entityColumnType)
            };

            if(customAttributes != null)
            {
                foreach (string customAttribute in customAttributes)
                {
                    property.CustomAttributes.Add(new CodeAttributeDeclaration
                    {
                        Name = customAttribute
                    });
                }
            }
            targetClass.Members.Add(property);
        }

        public void AddProperties(string pocoClassName, string propertyType, string contextName)
        {
            // Declare the read-only Width property.

            CodeMemberProperty property = new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = pocoClassName,
                HasGet = true,
                Type = new CodeTypeReference($"{propertyType}<{pocoClassName},{contextName}>")
            };
            property.GetStatements.Add(new CodeMethodReturnStatement(
                                        new CodeFieldReferenceExpression
                                        {
                                            FieldName = $"new MSSQLProviderContext<{pocoClassName},{contextName}>()"
                                        }));
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
