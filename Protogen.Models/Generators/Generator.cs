using System;
using System.Collections.Generic;
using System.Text;

namespace Protogen.Models.Generators
{
    public abstract class Generator
    {
        public abstract Dictionary<string, string> Generate(Project project);
    }
}
