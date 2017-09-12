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
                      .AppendLine($"using {_model.Project.Namespace ?? _model.Project.Name}.GraphQL;")
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
            _generator.AppendLine($"public {_model.Project.Name.Pascalize()}Type()")
                      .BeginBlock();

            foreach (var field in _model.AllFields)
            {
                _generator.AppendLine($"Field(\"{field.Name.Underscore()}\", x => x.{field.Name.Pascalize()}, nullable: {field.Null}).Description(@\"{field.Description}\");");
            }

            _generator.EndBlock();
        }
    }
}
