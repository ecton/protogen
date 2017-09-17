using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Protogen.Models.Generators.Csharp
{
    public class EFModel
    {
        private CodeGenerator _generator = new CodeGenerator();
        private Model _model;
        public EFModel(Model model)
        {
            _model = model;
        }

        public string Generate()
        {
            RenderUsingStatements();
            BeginClass();
            RenderFields();
            EndClass();
            return _generator.ToString();
        }

        private void RenderUsingStatements()
        {
            _generator.AppendLine("using System;")
                      .AppendLine("using System.Collections.Generic;")
                      .AppendLine("using System.ComponentModel.DataAnnotations;")
                      .AppendLine("using System.ComponentModel.DataAnnotations.Schema;")
                      .AppendLine();
        }

        private void BeginClass()
        {
            _generator.AppendLine($"namespace {_model.Project.Namespace ?? _model.Project.Name}.Models")
                      .BeginBlock()
                      .AppendLine($"[Table(\"{_model.Name.Underscore().Pluralize()}\")]")
                      .AppendLine($"public class {_model.Name.Pascalize()}")
                      .BeginBlock();
        }

        private void EndClass()
        {
            _generator.EndBlock() //End class
                      .EndBlock(); // End namespace
        }

        private void RenderFields()
        {
            foreach (var field in _model.AllFields)
            {
                RenderField(field);
                if (field.ForeignKey != null)
                {
                    RenderForeignKey(field);
                }
            }
            foreach (var field in _model.Project.AllModels
                                        .SelectMany(m => m.AllFields)
                                        .Where(f => f.ForeignKey != null 
                                                    && f.ForeignKey.RefersTo.Model == _model))
            {
                RenderInverseProperty(field);
            }
        }

        private void RenderField(ModelField field)
        {
            RenderAttributes(field);
            _generator.AppendLine($"public {CsharpGenerator.Type(field.ResolvedType, field.Null)} {field.Name.Pascalize()} {{ get; set; }}");
        }

        private void RenderAttributes(ModelField field)
        {
            RenderKeyAttribute(field);
            RenderRequiredAttribute(field);
            RenderColumnAttribute(field);
        }

        private void RenderKeyAttribute(ModelField field)
        {
            if (field.PrimaryKey)
            {
                _generator.AppendLine("[Key]");
            }
        }

        private void RenderRequiredAttribute(ModelField field)
        {
            if (!field.Null)
            {
                _generator.AppendLine("[Required]");
            }
        }

        private void RenderColumnAttribute(ModelField field)
        {
            _generator.Append($"[Column(\"{field.Name.Underscore()}\"");
            var dbType = FieldTypeToDb(field);
            if (dbType != null)
            {
                _generator.Append($", TypeName = \"{dbType}\"");
            }
            _generator.AppendLine(")]");
        }

        private void RenderForeignKey(ModelField field)
        {
            _generator.AppendLine($"[ForeignKey(nameof({field.Name.Pascalize()}))]")
                      .AppendLine($"public virtual {field.ForeignKey.RefersTo.Model.Name.Pascalize()} {field.AccessorName.Pascalize()} {{ get; set; }}");
        }

        private string FieldTypeToDb(ModelField field)
        {
            switch (field.ResolvedType.FieldType)
            {
                case FieldType.Date:
                    return "date";
            }
            return null;
        }

        private void RenderInverseProperty(ModelField field)
        {
            if (field.ResolvedInverseName != null)
            {
                var accessorName = field.AccessorName.Pascalize()
                                        .Replace(field.ForeignKey.RefersTo.Model.Name.Pascalize(), _model.Name.Pascalize());
                _generator.AppendLine($"[InverseProperty(nameof({field.AccessorName.Pascalize()}))]")
                          .AppendLine($"public virtual ICollection<{field.Model.Name.Pascalize()}> {field.ResolvedInverseName} {{ get; set; }}");
            }
        }
    }
}
