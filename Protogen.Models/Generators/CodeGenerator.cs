using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protogen.Models.Generators
{
    class CodeGenerator
    {
        private StringBuilder _builder = new StringBuilder();
        private string _blockOpener, _blockCloser, _indentation;

        public CodeGenerator(string blockOpener = "{", string blockCloser = "}", string indentation = "    ")
        {
            _blockOpener = blockOpener;
            _blockCloser = blockCloser;
            _indentation = indentation;
        }

        private bool _isStartOfLine = true;
        private string _currentWhitespace = "";
        private void AddWhitespaceToLineIfNeeded()
        {
            if (_isStartOfLine)
            {
                _isStartOfLine = false;
                _builder.Append(_currentWhitespace);
            }
        }
        public CodeGenerator BeginBlock(string opener = null)
        {
            AddWhitespaceToLineIfNeeded();
            _currentWhitespace += _indentation;
            AppendLine(opener ?? _blockOpener);
            return this;
        }

        public CodeGenerator EndBlock(string closer = null)
        {
            _currentWhitespace = _currentWhitespace.Substring(_indentation.Length);
            AddWhitespaceToLineIfNeeded();
            AppendLine(closer ?? _blockCloser);
            return this;
        }

        public CodeGenerator Append(string str)
        {
            var lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines.Take(lines.Length - 1)) AppendLine(line);

            AddWhitespaceToLineIfNeeded();
            _builder.Append(lines.Last());

            return this;
        }

        public CodeGenerator AppendLine(string str = "")
        {
            var lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                AddWhitespaceToLineIfNeeded();
                _builder.AppendLine(line);
                _isStartOfLine = true;
            }
            return this;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
