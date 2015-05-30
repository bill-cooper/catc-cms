using System;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;

namespace ceenq.com.LinuxCommands.Commands
{
    public class ListFilesCommand : Component, IListFilesCommand
    {
        private string _path;

        public void Initialize(string[] parameters)
        {
            if (parameters.Length != 1)
                throw new OrchardException(T("ListFilesCommand failed"), new ArgumentException("ListFilesCommand expected exactly 1 arguments", "parameters"));
            _path = parameters[0];
        }

        public string CommandText
        {
            get { return string.Format("sudo find {0} -type f", _path); }
        }
    }
}