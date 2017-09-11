using System;
using System.Collections.Generic;
using System.Text;

namespace Protogen.Models
{
    public class QLField
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ResolvedType ResolvedType { get; set; }
        public string Description { get; set; }
        public object Default { get; set; }
        public QLField[] Arguments { get; set; }

        public Project Project { get; set; }
        public void Preprocess()
        {
            ResolvedType = new ResolvedType(Project, Type);
            if (Arguments != null)
            {
                foreach (var argument in Arguments)
                {
                    argument.Project = Project;
                    argument.Preprocess();
                }
            }
        }
    }
}
