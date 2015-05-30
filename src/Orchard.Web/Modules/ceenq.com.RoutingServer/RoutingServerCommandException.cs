using Orchard;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ceenq.com.RoutingServer
{
    [Serializable]
    public class RoutingServerCommandException : OrchardException
    {
        private readonly string _command;
        private readonly int _commandExitStatus;
        private readonly string _commandError;
        public RoutingServerCommandException(LocalizedString message, string command, int commandExitStatus, string commandError)
            : base(message)
        {
            _command = command;
            _commandExitStatus = commandExitStatus;
            _commandError = commandError;
        }

        public string Command { get { return _command; } }
        public int CommandExitStatus { get { return _commandExitStatus; } }
        public string CommandError { get { return _commandError; } }
    }
}