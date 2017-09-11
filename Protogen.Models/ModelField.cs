using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace Protogen.Models
{
    public class ModelField
    {
        public String Name { get; set; }
        public bool PrimaryKey { get; set; }
        public string Type { get; set; }
        public ResolvedType ResolvedType { get; set; }
        public string RelatedField { get; set; }
        public bool Null { get; set; }
        public object Default { get; set; }

        public Model Model { get; set; }

        public static implicit operator ModelField(string type)
        {
            return new ModelField()
            {
                Type = type
            };
        }

        public void Preprocess()
        {
            ResolvedType = new ResolvedType(Model.Project, Type);
        }
    }
}
