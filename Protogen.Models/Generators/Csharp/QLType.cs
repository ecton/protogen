using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Protogen.Models.Generators.Csharp
{
    class QLType
    {
        private Model _model;
        private CodeGenerator _generator = new CodeGenerator();
        public QLType(Model model)
        {
            _model = model;
        }

        public string Generate()
        {
            RenderUsingStatements();
            BeginClass();
            RenderConstructor();
            EndClass();
            return _generator.ToString();
        }

        private void RenderUsingStatements()
        {
            _generator.AppendLine("using System;")
                      .AppendLine("using GraphQL;")
                      .AppendLine("using GraphQL.Types;")
                      .AppendLine($"using {_model.Project.Namespace ?? _model.Project.Name}.Models;")
                      .AppendLine();
        }

        private void BeginClass()
        {
            _generator.AppendLine($"namespace {_model.Project.Namespace ?? _model.Project.Name}.GraphQL")
                      .BeginBlock()
                      .AppendLine($"public class {_model.Name.Pascalize()}Type : ObjectGraphType<{_model.Name.Pascalize()}>")
                      .BeginBlock();
        }

        private void EndClass()
        {
            _generator.EndBlock() //End class
                      .EndBlock(); // End namespace
        }

        private void RenderConstructor()
        {
            _generator.AppendLine($"public {_model.Name.Pascalize()}Type()")
                      .BeginBlock();

            foreach (var field in _model.AllFields)
            {
                if (field.PrimaryKey && _model.HasSimplePrimaryKey)
                {
                    _generator.AppendLine($"Id(x => x.{field.Name.Pascalize()});");
                }
                else
                {
                    if (field.ForeignKey != null)
                    {
                        _generator.AppendLine($"Field<{field.ForeignKey.RefersTo.Model.Name.Pascalize()}>(\"{field.AccessorName.Camelize()}\", @\"{field.Description}\", resolve: ctx => ")
                                  .BeginBlock()
                                  .AppendLine($"var schemaContext = ({_model.Project.Name.Pascalize()}Schema.Context)ctx;")
                                  .AppendLine($"return schemaContext.Database.{field.ForeignKey.RefersTo.Model.Name.Pascalize().Pluralize()}.Where(x => x.{field.ForeignKey.RefersTo.Name.Pascalize()} == ctx.Source.{field.Name.Pascalize()}).FirstOrDefault();")
                                  .EndBlock("});");
                    }
                    else
                    {
                        _generator.AppendLine($"Field<{CsharpGenerator.Type(field.ResolvedType, field.Null)}>(\"{field.Name.Camelize()}\", @\"{field.Description}\", resolve: ctx => ctx.Source.{field.Name.Pascalize()});");
                    }
                }
            }

            _generator.EndBlock();
        }
    }
}
