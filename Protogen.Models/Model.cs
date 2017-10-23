using System;
using System.Collections.Generic;
using System.Linq;

namespace Protogen.Models
{
    public class Model
    {
        public string Name { get; set; }
        public Dictionary<string, ModelField> Fields
        {
            set
            {
                foreach (var finfo in value)
                {
                    finfo.Value.Name = finfo.Key;
                    _fields.Add(finfo.Value);
                }
            }
        }
        public QLField[] Queries { get; set; }
        public QLField[] Mutations { get; set; }
        public Project Project { get; set; }
        public string Description { get; set; }
        public bool Database { get; set; } = true;
        public bool GraphQL { get; set; } = true;

        private List<ModelField> _fields = new List<ModelField>();
        public IEnumerable<ModelField> AllFields { get => _fields.OrderBy(f => !f.PrimaryKey).ThenBy(f => f.Name); }

        public IEnumerable<ModelField> PrimaryKeys { get => AllFields.Where(f => f.PrimaryKey); }
        public bool HasSimplePrimaryKey { get => PrimaryKeys.Count() == 1; }

        public void Preprocess()
        {
            foreach (var field in AllFields)
            {
                field.Model = this;
                field.Preprocess();
            }

            foreach (var field in Queries ?? Enumerable.Empty<QLField>())
            {
                field.Project = Project;
                field.Type = Name;
                field.Preprocess();
            }

            foreach (var field in Mutations ?? Enumerable.Empty<QLField>())
            {
                field.Project = Project;
                field.Type = Name;
                field.Preprocess();
            }
        }
        public void ResolveAutoTypes()
        {
            foreach (var field in AllFields)
            {
                field.ResolveAutoTypes();
            }

            foreach (var field in Queries ?? Enumerable.Empty<QLField>())
            {
                field.ResolveAutoTypes();
            }

            foreach (var field in Mutations ?? Enumerable.Empty<QLField>())
            {
                field.ResolveAutoTypes();
            }
        }

        public IEnumerable<ModelField> ReferencingFields
        {
            get
            {
                return Project.AllModels
                              .SelectMany(m => m.AllFields)
                              .Where(f => f.ForeignKey != null
                                       && f.ForeignKey.RefersTo.Model == this);
            }
        }
    }
}
