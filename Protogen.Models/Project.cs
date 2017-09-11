using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Preprocess();
            var generator = new CsharpGenerator();
            return generator.Generate(this);
        }

        private void Preprocess()
        {
            foreach (var model in _models)
            {
                model.Project = this;
                model.Preprocess();
            }
        }

        public Model ModelNamed(string name)
        {
            try
            {
                return AllModels.Where(m => m.Name == name).First();
            }
            catch
            {
                throw new ArgumentException($"Model not found: {name}");
            }
        }
        public IEnumerable<QLField> AllQueries
        {
            get => _models.SelectMany(m => m.Queries ?? Enumerable.Empty<QLField>());
        }
        public IEnumerable<QLField> AllMutations
        {
            get => _models.SelectMany(m => m.Mutations ?? Enumerable.Empty<QLField>());
        }
    }
}
