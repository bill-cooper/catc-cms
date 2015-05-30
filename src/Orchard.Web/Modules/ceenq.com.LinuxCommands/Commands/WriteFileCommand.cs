using System;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;

namespace ceenq.com.LinuxCommands.Commands
{
    public class WriteFileCommand :Component, IWriteFileCommand
    {
        private string _path;
        private string _content;
        public void Initialize(string[] parameters)
        {
            if(parameters.Length != 2)
                throw new OrchardException(T("WriteFileCommand failed"), new ArgumentException("WriteFileCommand expected exactly 2 arguments", "parameters"));
            _path = parameters[0];
            _content = parameters[1];
        }

        public string CommandText
        {
            get
            {
                var escapedContent = _content
                .Replace(@"\", @"\\")
                .Replace(@"$", @"\$")
                .Replace(@"'", @"\047")
                .Replace("\"", @"\042")
                .Replace(@"?", @"\077");
                return string.Format("sudo printf \"{0}\" | sudo tee {1}", escapedContent, _path);
            }
        }
    }
}