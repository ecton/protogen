using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Protogen.Models.Generators.Csharp
{
    public class CsharpGenerator : Generator
    {
        public override Dictionary<string, string> Generate(Project project)
        {
            var results = new Dictionary<string, string>();
            results[$"{project.Name.Pascalize()}DbContext.cs"] = new EFDbContext(project).Generate();
            foreach (var model in project.AllModels)
            {
                results[$"{model.Name.Pascalize()}.cs"] = new EFModel(project, model).Generate();
            }

            if (project.AllQueries.Any() || project.AllMutations.Any())
            {
                results[$"{project.Name.Pascalize()}Schema.cs"] = new QLSchema(project).Generate();
                if (project.AllQueries.Any())
                {
                    results[$"{project.Name.Pascalize()}QueryBase.cs"] = new QLFieldsClass(project, "Query", project.AllQueries).Generate();
                }
            }

            return results;
        }

        public static string Type(ResolvedType type, bool nullable)
        {
            string cType = null;
            bool inherentlyNullable = false;
            switch (type.FieldType)
            {
                case FieldType.Text:
                case FieldType.String:
                    cType = "string";
                    inherentlyNullable = true;
                    break;
                case FieldType.Boolean:
                    cType = "bool";
                    break;
                case FieldType.Integer:
                    cType = "int";
                    break;
                case FieldType.BigInteger:
                    cType = "long";
                    break;
                case FieldType.Float:
                    cType = "float";
                    break;
                case FieldType.Double:
                    cType = "double";
                    break;
                case FieldType.Date:
                    cType = "NpgsqlDate";
                    break;
                case FieldType.DateTime:
                    cType = "NpgsqlDateTime";
                    break;
                case FieldType.Time:
                    cType = "NpgsqlDateTime";
                    break;
                case FieldType.Guid:
                    cType = "Guid";
                    break;
                case FieldType.Model:
                    cType = type.Model.Name; // TODO: For this to work for both schemas and EF, we need to be able to add a suffix
                    inherentlyNullable = true;
                    break;
                case FieldType.Auto:
                    cType = "long"; // TODO: Look up type, especially for foreign keys
                    break;
                default:
                    throw new NotImplementedException($"Unknown field type {type.FieldType}");
            }
            if (nullable && !inherentlyNullable)
            {
                cType = cType + "?";
            }
            return cType;
        }
    }
}
