using System;
using Orchard;
using Orchard.Localization;

namespace ceenq.com.Core.Infrastructure.Compute
{
    [Serializable]
    public class ServerCommandException : OrchardFatalException
    {
        private readonly string _command;
        private readonly int _commandExitStatus;
        private readonly string _commandError;
        private readonly string _commandResult;
        public ServerCommandException(LocalizedString message, string command, int commandExitStatus, string commandError, string commandResult)
            : base(message)
        {
            _command = command;
            _commandExitStatus = commandExitStatus;
            _commandError = commandError;
            _commandResult = commandResult;
        }

        public string Command { get { return _command; } }
        public int CommandExitStatus { get { return _commandExitStatus; } }
        public string CommandError { get { return _commandError; } }
        public string CommandResult { get { return _commandResult; } }
    }
}