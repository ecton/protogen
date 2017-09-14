using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace Protogen.Models
{
    public class ModelField
    {
        public String Name { get; set; }
        public string Description { get; set; }
        public bool PrimaryKey { get; set; }
        public ForeignKey ForeignKey { get; set; }
        public string InverseName { get; set; }
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
            if (ForeignKey != null)
            {
                ForeignKey.ModelField = this;
                ForeignKey.Preprocess();
            }
        }

        public void ResolveAutoTypes()
        {
            if (ResolvedType.FieldType == FieldType.Auto)
            {
                if (ForeignKey != null)
                {
                    ResolvedType = ForeignKey.RefersTo.ResolvedType;
                }
                else if (PrimaryKey)
                {
                    ResolvedType = new ResolvedType()
                    {
                        FieldType = FieldType.Integer
                    };
                }
                else
                {
                    throw new ArgumentException($"Can't determine automatic type of {Model.Name}.{Name}");
                }
            }
        }
        
        public string AccessorName
        {
            get
            {
                if (Name.Substring(Name.Length - 2, 2).ToLower() == "id")
                {
                    return Name.Substring(0, Name.Length - 2);
                }
                return Name;
            }
        }

        public string ResolvedInverseName
        {
            get
            {
                if (InverseName != null) return InverseName.Pluralize();
                switch (AccessorName.Pascalize())
                {
                    case "Parent": return "Children";
                }
                return null;
            }
        }
    }
}
