using System;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;

namespace ceenq.com.LinuxCommands.Commands
{
    public class ReadFileCommand : Component, IReadFileCommand
    {
        private string _path;
        private string _numberOfLinesToReadFromFromEof;

        public void Initialize(string[] parameters)
        {
            if (parameters.Length != 2)
                throw new OrchardException(T("ReadFileCommand failed"), new ArgumentException("ReadFileCommand expected exactly 2 arguments", "parameters"));
            _path = parameters[0];
            _numberOfLinesToReadFromFromEof = parameters[1];
        }

        public string CommandText
        {
            get { return string.Format("sudo tail -{1} {0}", _path, _numberOfLinesToReadFromFromEof); }
        }
    }
}