using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace Protogen.Models.Generators.Csharp
{
    public class CsharpGenerator : Generator
    {
        public override Dictionary<string, string> Generate(Project project)
        {
            var results = new Dictionary<string, string>();
            results[$"{project.Name.Pascalize()}DbContext.cs"] = new DbContext(project).Generate();
            foreach (var model in project.AllModels)
            {
                results[$"{model.Name.Pascalize()}.cs"] = new EFModel(project, model).Generate();
            }
            return results;
        }
    }
}
