using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace Protogen.Models.Generators.Csharp
{
    class EFDbContext
    {
        private Project _project;
        private CodeGenerator _generator = new CodeGenerator();
        public EFDbContext(Project project)
        {
            _project = project;
        }


        public string Generate()
        {
            RenderUsingStatements();
            BeginClass();
            RenderModels();
            RenderOnModelCreating();
            EndClass();
            return _generator.ToString();
        }

        private void RenderUsingStatements()
        {
            _generator.AppendLine("using Microsoft.EntityFrameworkCore;")
                      .AppendLine("using Microsoft.EntityFrameworkCore.Infrastructure;")
                      .AppendLine("using Microsoft.EntityFrameworkCore.Migrations;")
                      .AppendLine("using System.Linq;")
                      .AppendLine();
        }

        private void BeginClass()
        {
            _generator.AppendLine($"namespace {_project.Namespace ?? $"{_project.Name}.Models"}")
                      .BeginBlock()
                      .AppendLine($"public class {_project.Name.Pascalize()}DbContext : DbContext")
                      .BeginBlock();
        }

        private void EndClass()
        {
            _generator.EndBlock() //End class
                      .EndBlock(); // End namespace
        }

        private void RenderModels()
        {
            foreach (var model in _project.AllModels)
            {
                _generator.AppendLine($"public DbSet<{model.Name.Pascalize()}> {model.Name.Pluralize().Pascalize()} {{ get; set; }}");
            }
        }

        private void RenderOnModelCreating()
        {
            _generator.AppendLine()
                      .AppendLine("protected override void OnModelCreating(ModelBuilder modelBuilder)")
                      .BeginBlock();

            foreach (var model in _project.AllModels)
            {
            }

            _generator.AppendLine("base.OnModelCreating(modelBuilder);")
                      .EndBlock();
        }
    }
}
