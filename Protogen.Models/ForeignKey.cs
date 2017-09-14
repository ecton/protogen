using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Protogen.Models
{
    public class ForeignKey
    {
        public ModelField ModelField { get; set; }
        public string ModelName { get; set; }
        public string FieldName { get; set; }

        public ModelField RefersTo { get; set; }

        public static implicit operator ForeignKey(string reference)
        {
            var parts = reference.Split('.');
            if (parts.Length != 2) throw new ArgumentOutOfRangeException($"{reference} is not a valid foreign key reference");

            return new ForeignKey()
            {
                ModelName = parts[0],
                FieldName = parts[1]
            };
        }

        public void Preprocess()
        {
            var model = ModelField.Model.Project.AllModels.Where(m => m.Name.Pascalize() == ModelName.Pascalize()).FirstOrDefault();
            if (model == null) throw new ArgumentException($"Invalid foreign key model: {ModelName}");
            RefersTo = model.AllFields.Where(f => f.Name.Pascalize() == FieldName.Pascalize()).FirstOrDefault();
            if (RefersTo == null) throw new ArgumentException($"Invalid field name in foreign key. {model.Name} does not contain field {FieldName}");
        }
    }
}
