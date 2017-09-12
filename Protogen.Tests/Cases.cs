using System;
using System.IO;
using System.Reflection;
using Xunit;
using Protogen.Models;

namespace Protogen.Tests
{
    public class Cases
    {
        private string NormalizeLineEndings(string str)
        {
            // Normalize line endings for cross platform
            // Replace CRLF with something unique
            str = str.Replace("\r\n", "\0\0\0\0");
            // Replace CR or LF with the same unique value
            str = str.Replace("\r", "\0\0\0\0");
            str = str.Replace("\n", "\0\0\0\0");
            // Replace with Environment.NewLine
            return str.Replace("\0\0\0\0", Environment.NewLine);
        }
        [Fact]
        public void RunCases()
        {
            var netcoreDir = Directory.GetParent(GetType().GetTypeInfo().Assembly.Location);
            var releaseDir = netcoreDir.Parent;
            var binDir = releaseDir.Parent;
            var projectDir = binDir.Parent;
            var casesDir = projectDir.GetDirectories("Cases")[0];

            foreach (var caseDir in casesDir.GetDirectories())
            {
                var caseName = caseDir.Name;
                var projectPath = caseDir.GetFiles($"{caseName}.json")[0];
                var project = new Project(projectPath.FullName);
                Assert.Equal(caseName, project.Name);
                var outputsDir = caseDir.GetDirectories("Outputs")[0];
                foreach (var fileInfo in project.GenerateFiles())
                {
                    var fileInfoPath = string.Join(Path.DirectorySeparatorChar.ToString(), fileInfo.Key.Split('/'));
                    var caseFile = outputsDir.GetFiles($"{fileInfoPath}")[0];
                    var expectedContents = File.ReadAllText(caseFile.FullName);
                    expectedContents = NormalizeLineEndings(expectedContents);
                    Assert.Equal(expectedContents, fileInfo.Value);
                }
            }
        }
    }
}
