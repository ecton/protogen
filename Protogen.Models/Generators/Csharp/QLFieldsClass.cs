using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Protogen.Models.Generators.Csharp
{
    class QLFieldsClass
    {
        private Project _project;
        private string _suffix;
        private IEnumerable<QLField> _fields;
        private CodeGenerator _generator = new CodeGenerator();
        public QLFieldsClass(Project project, string suffix, IEnumerable<QLField> fields)
        {
            _project = project;
            _suffix = suffix;
            _fields = fields;
        }

        public string Generate()
        {
            RenderUsingStatements();
            BeginClass();
            RenderConstructor();
            RenderStubs();
            EndClass();
            return _generator.ToString();
        }

        private void RenderUsingStatements()
        {
            _generator.AppendLine("using System;")
                      .AppendLine("using GraphQL;")
                      .AppendLine("using GraphQL.Types;")
                      .AppendLine();
        }

        private void BeginClass()
        {
            _generator.AppendLine($"namespace {_project.Namespace ?? _project.Name}.GraphQL")
                      .BeginBlock()
                      .AppendLine($"public abstract class {_project.Name.Pascalize()}{_suffix}Base : ObjectGraphType")
                      .BeginBlock();
        }

        private void EndClass()
        {
            _generator.EndBlock() //End class
                      .EndBlock(); // End namespace
        }

        private void RenderConstructor()
        {
            _generator.AppendLine($"public {_project.Name.Pascalize()}{_suffix}Base()")
                      .BeginBlock();
            
            foreach (var field in _fields)
            {
                _generator.AppendLine($"Field<{CsharpGenerator.Type(field.ResolvedType, false)}Type>(")
                          .IncreaseIndentation()
                          .AppendLine($"\"{field.Name.Camelize()}\",")
                          .AppendLine($"resolve: Resolve{field.Name.Pascalize()},");
                
                _generator.AppendLine($"description: @\"{field.Description?.Replace("\"", "\"\"") ?? ""}\",");

                if (field.Arguments != null)
                {
                    _generator.AppendLine("arguments: new QueryArguments(")
                              .IncreaseIndentation();

                    foreach (var arg in field.Arguments)
                    {
                        RenderArgument(arg, arg == field.Arguments.Last());
                    }

                    _generator.DecreaseIndentation()
                              .AppendLine(")");
                }
                else
                {
                    _generator.AppendLine("arguments: new QueryArguments()");
                }

                _generator.DecreaseIndentation()
                          .AppendLine(");");

            }

            _generator.EndBlock();
        }

        private void RenderArgument(QLField arg, bool isLast)
        {
            _generator.AppendLine($"new QueryArgument(typeof({CsharpGenerator.Type(arg.ResolvedType, false)}).GetGraphTypeFromType({(arg.Default != null).ToString().ToLower()}))")
                      .BeginBlock()
                      .AppendLine($"Name = \"{arg.Name.Camelize()}\",")
                      .AppendLine($"DefaultValue = {arg.Default},")
                      .AppendLine($"Description = @\"{arg.Description?.Replace("\"", "\"\"") ?? ""}\"")
                      .EndBlock(isLast ? "}" : "},");
        }

        private void RenderStubs()
        {
            foreach (var field in _fields)
            {
                _generator.AppendLine($"public abstract object Resolve{field.Name.Pascalize()}(ResolveFieldContext<object> context);");
            }
        }
    }
}
