using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace Protogen.Models
{
    public class ModelField
    {
        public enum FieldType
        {
            Auto,
            String,
            Text,
            Boolean,
            Integer,
            BigInteger,
            Float,
            Double,
            Date,
            DateTime,
            Time,
            Guid
        }
        public String Name { get; set; }
        public bool PrimaryKey { get; set; }
        public FieldType Type { get; set; }
        public bool Null { get; set; }
        public object Default { get; set; }

        public static implicit operator ModelField(string type)
        {
            return new ModelField()
            {
                Type = (FieldType)Enum.Parse(typeof(FieldType), type.Pascalize())
            };
        }

        public void Preprocess()
        {

        }
    }
}
