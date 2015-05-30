﻿using System;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;

namespace ceenq.com.LinuxCommands.Commands
{
    public class DeleteCommand :Component, IDeleteCommand
    {
        private string _path;
        public void Initialize(string[] parameters)
        {
            if (parameters.Length != 1)
                throw new OrchardException(T("DeleteCommand failed"), new ArgumentException("DeleteCommand expected exactly 1 arguments", "parameters"));
            _path = parameters[0];
        }

        public string CommandText {
            get { return string.Format("sudo rm -fr {0}", _path); }
        }
    }
}