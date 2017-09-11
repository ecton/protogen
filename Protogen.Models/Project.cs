using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Protogen.Models.Generators.Csharp;

namespace Protogen.Models
{
    public class Project
    {
        public string Path { get; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string[] Models
        {
            set
            {
                foreach (var model in value) LoadModel(model);
            }
        }
        private List<Model> _models = new List<Model>();
        [JsonIgnore]
        public IEnumerable<Model> AllModels { get => _models; }
        public Project(string path)
        {
            Path = path;
            var contents = File.ReadAllText(path, Encoding.UTF8);
            JsonConvert.PopulateObject(contents, this);
        }

        private void LoadModel(string modelName)
        {
            var path = Directory.GetParent(Path).GetFiles($"{modelName}.json")[0];
            var contents = File.ReadAllText(path.FullName, Encoding.UTF8);
            var model = JsonConvert.DeserializeObject<Model>(contents);
            model.Name = modelName;
            _models.Add(model);
        }

        public Dictionary<string, string> GenerateFiles()
        {
            // TODO: Look up language based on project setting
            var generator = new CsharpGenerator();
            return generator.Generate(this);
        }

        private void Preprocess()
        {
            foreach (var model in _models)
            {
                model.Preprocess();
            }
        }
    }
}
