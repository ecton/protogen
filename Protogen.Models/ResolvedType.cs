using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace Protogen.Models
{
    public struct ResolvedType
    {
        public FieldType FieldType;
        public Model Model;

        public ResolvedType(Project project, string typeName)
        {
            Model = null;
            FieldType = FieldType.Auto;
            if (!string.IsNullOrEmpty(typeName) && !Enum.TryParse<FieldType>(typeName.Pascalize(), out FieldType))
            {
                Model = project.ModelNamed(typeName);
                FieldType = FieldType.Model;
            }
        }
    }
}
