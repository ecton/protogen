using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using Protogen.Models;

namespace Protogen
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication(true);
            app.Command("gen", target => GenerateCommand(target));
            app.Execute(args);
        }

        private static CommandOption _projectFile;
        private static CommandOption _outputFolder;
        private static CommandOption _tests;
        private static void GenerateCommand(CommandLineApplication command)
        {
            _projectFile = command.Option("-p|--project <project>", "Path to the project", CommandOptionType.SingleValue);
            _outputFolder = command.Option("-o|--output <folder>", "Folder to output generated files", CommandOptionType.SingleValue);
            _tests = command.Option("--tests", "Overrides project and output to regenerate all the test case files.", CommandOptionType.NoValue);
            command.OnExecute(() => Generate());
        }

        struct GenerateArguments
        {
            public string Project;
            public string Output;
        }
        private static int Generate()
        {
            var args = new List<GenerateArguments>();
            if (_tests.HasValue())
            {
                args.AddRange(FindAllTestCases());
            }
            else if (!_projectFile.HasValue())
            {
                args.Add(new GenerateArguments()
                {
                    Project = Path.Combine(Directory.GetCurrentDirectory(), "Protogen", "Protogen.json"),
                    Output = Directory.GetParent(Directory.GetCurrentDirectory()).FullName
                });
            }
            if (_projectFile.HasValue())
            {
                args.Add(new GenerateArguments
                {
                    Project = _projectFile.Value(),
                    Output = _outputFolder.HasValue() ? _outputFolder.Value() : Directory.GetParent(_projectFile.Value()).FullName
                });
            }
            foreach (var gen in args)
            {
                Console.WriteLine($"Generating {gen.Project}");
                var project = new Project(gen.Project);
                Directory.CreateDirectory(gen.Output);
                foreach (var fileInfo in project.GenerateFiles())
                {
                    var pathParts = fileInfo.Key.Split('/');
                    var outputFolder = Path.Combine(gen.Output, pathParts[0]);
                    var outputFile = Path.Combine(outputFolder, pathParts[1]);
                    Directory.CreateDirectory(outputFolder);
                    File.WriteAllBytes(outputFile, Encoding.UTF8.GetBytes(fileInfo.Value));
                }
            }
            return 0;
        }

        private static IEnumerable<GenerateArguments> FindAllTestCases()
        {
            var netcoreDir = Directory.GetParent(typeof(Program).GetTypeInfo().Assembly.Location);
            var releaseDir = netcoreDir.Parent;
            var binDir = releaseDir.Parent;
            var projectDir = binDir.Parent;
            var solutionDir = projectDir.Parent;
            var testsDir = solutionDir.GetDirectories("Protogen.Tests")[0];
            var casesDir = testsDir.GetDirectories("Cases")[0];

            foreach (var caseDir in casesDir.GetDirectories())
            {
                var caseName = caseDir.Name;
                var projectPath = caseDir.GetFiles($"{caseName}.json")[0];
                yield return new GenerateArguments
                {
                    Project = projectPath.FullName,
                    Output = Path.Combine(caseDir.FullName, "Outputs")
                };
            }
        }
    }
}
