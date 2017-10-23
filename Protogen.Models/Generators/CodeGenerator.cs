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
        private void AddWhitespaceToLineIfNeeded(bool isCodeLine)
        {
            if (_isStartOfLine)
            {
                if (_insertEmptyLineBeforeNextCodeLine && isCodeLine)
                {
                    _builder.AppendLine();
                }
                _insertEmptyLineBeforeNextCodeLine = false;

                _isStartOfLine = false;
                _builder.Append(_currentWhitespace);
            }
        }
        public CodeGenerator IncreaseIndentation()
        {
            _currentWhitespace += _indentation;
            return this;
        }

        public CodeGenerator DecreaseIndentation()
        {
            _currentWhitespace = _currentWhitespace.Substring(_indentation.Length);
            return this;
        }


        public CodeGenerator BeginBlock(string opener = null)
        {
            AppendLine(opener ?? _blockOpener, false);
            IncreaseIndentation();
            return this;
        }

        public CodeGenerator EndBlock(string closer = null)
        {
            DecreaseIndentation();
            AppendLine(closer ?? _blockCloser, false);
            return this;
        }

        public CodeGenerator Append(string str, bool isCodeLine = true)
        {
            var lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines.Take(lines.Length - 1)) AppendLine(line);

            AddWhitespaceToLineIfNeeded(isCodeLine);
            _builder.Append(lines.Last());

            return this;
        }

        public CodeGenerator AppendLine(string str = "", bool isCodeLine = true)
        {
            var lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                Append(line, isCodeLine);
                _builder.AppendLine();
                _isStartOfLine = true;
            }
            return this;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        private bool _insertEmptyLineBeforeNextCodeLine = false;
        public CodeGenerator EnsureEmptyLine()
        {
            _insertEmptyLineBeforeNextCodeLine = true;
            return this;
        }
    }
}
