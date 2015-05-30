using System;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;

namespace ceenq.com.LinuxCommands.Commands
{
    public class MoveCommand :Component, IMoveCommand
    {
        private string _oldPath;
        private string _newPath;
        public void Initialize(string[] parameters)
        {
            if(parameters.Length != 2)
                throw new OrchardException(T("MoveCommand failed"), new ArgumentException("MoveCommand expected exactly 2 arguments", "parameters"));
            _oldPath = parameters[0];
            _newPath = parameters[1];
        }

        public string CommandText
        {
            get
            {
                return string.Format("sudo mv {0} {1}", _oldPath, _newPath);
            }
        }
    }
}