using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Protogen.Models.Generators.Csharp
{
    class QLSchema
    {
        private Project _project;
        private CodeGenerator _generator = new CodeGenerator();
        public QLSchema(Project project)
        {
            _project = project;
        }


        public string Generate()
        {
            RenderUsingStatements();
            BeginClass();
            RenderContextClass();
            RenderConstructor();
            EndClass();
            return _generator.ToString();
        }

        private void RenderUsingStatements()
        {
            _generator.AppendLine("using System;")
                      .AppendLine("using GraphQL;")
                      .AppendLine("using GraphQL.Types;")
                      .AppendLine($"using {_project.Namespace ?? _project.Name}.Models;")
                      .AppendLine();
        }

        private void BeginClass()
        {
            _generator.AppendLine($"namespace {_project.Namespace ?? _project.Name}.GraphQL")
                      .BeginBlock()
                      .AppendLine($"public class {_project.Name.Pascalize()}Schema : Schema")
                      .BeginBlock();
        }

        private void EndClass()
        {
            _generator.EndBlock() //End class
                      .EndBlock(); // End namespace
        }

        private void RenderConstructor()
        {
            _generator.AppendLine($"public {_project.Name.Pascalize()}Schema(Func<Type, GraphType> resolveType) : base(resolveType)")
                      .BeginBlock();
            
            if (_project.AllQueries.Count() > 0) {
                _generator.AppendLine($"Query = ({_project.Name.Pascalize()}Query)resolveType(typeof({_project.Name.Pascalize()}Query));");
            }

            if (_project.AllMutations.Count() > 0)
            {
                _generator.AppendLine($"Mutation = ({_project.Name.Pascalize()}Mutation)resolveType(typeof({_project.Name.Pascalize()}Mutation));");
            }

            _generator.EndBlock();
        }

        private void RenderContextClass()
        {
            _generator.AppendLine($"public class Context")
                      .BeginBlock()
                      .AppendLine($"public {_project.Name.Pascalize()}DbContext Database {{ get; set; }}")
                      .EndBlock();
        }
    }
}
