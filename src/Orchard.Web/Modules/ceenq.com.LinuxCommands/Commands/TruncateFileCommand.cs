using System;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;

namespace ceenq.com.LinuxCommands.Commands
{
    public class TruncateFileCommand :Component, ITruncateFileCommand
    {
        private string _path;
        public void Initialize(string[] parameters)
        {
            if (parameters.Length != 1)
                throw new OrchardException(T("TruncateFileCommand failed"), new ArgumentException("TruncateFileCommand expected exactly 1 arguments", "parameters"));
            _path = parameters[0];
        }
        public string CommandText { get { return string.Format("sudo truncate -s 0 {0}", _path); } }
    }
}