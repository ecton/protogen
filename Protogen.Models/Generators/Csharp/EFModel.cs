using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace Protogen.Models.Generators.Csharp
{
    public class EFModel
    {
        private CodeGenerator _generator = new CodeGenerator();
        private Project _project;
        private Model _model;
        public EFModel(Project project, Model model)
        {
            _project = project;
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
                      .AppendLine("using NpgsqlTypes;")
                      .AppendLine();
        }

        private void BeginClass()
        {
            _generator.AppendLine($"namespace {_project.Namespace ?? $"{_project.Name}.Models"}")
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
            }
        }

        private void RenderField(ModelField field)
        {
            RenderAttributes(field);
            _generator.AppendLine($"public {FieldTypeToCsharp(field)} {field.Name.Pascalize()} {{ get; set; }}");
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

        private string FieldTypeToCsharp(ModelField field)
        {
            string cType = null;
            bool inherentlyNullable = false;
            switch (field.Type)
            {
                case ModelField.FieldType.Text:
                case ModelField.FieldType.String:
                    cType = "string";
                    inherentlyNullable = true;
                    break;
                case ModelField.FieldType.Boolean:
                    cType = "bool";
                    break;
                case ModelField.FieldType.Integer:
                    cType = "int";
                    break;
                case ModelField.FieldType.BigInteger:
                    cType = "long";
                    break;
                case ModelField.FieldType.Float:
                    cType = "float";
                    break;
                case ModelField.FieldType.Double:
                    cType = "double";
                    break;
                case ModelField.FieldType.Date:
                    cType = "NpgsqlDate";
                    break;
                case ModelField.FieldType.DateTime:
                    cType = "NpgsqlDateTime";
                    break;
                case ModelField.FieldType.Time:
                    cType = "NpgsqlDateTime";
                    break;
                case ModelField.FieldType.Guid:
                    cType = "Guid";
                    break;
                case ModelField.FieldType.Auto:
                    cType = "long"; // TODO: Look up type, especially for foreign keys
                    break;
                default:
                    throw new NotImplementedException($"Unknown field type {field.Type}");
            }
            if (field.Null && !inherentlyNullable)
            {
                cType = cType + "?";
            }
            return cType;
        }

        private string FieldTypeToDb(ModelField field)
        {
            switch (field.Type)
            {
                case ModelField.FieldType.DateTime:
                    return "timestamptz";
            }
            return null;
        }
    }
}
