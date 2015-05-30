using System;
using System.Collections.Specialized;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;

namespace ceenq.com.LinuxCommands.Commands
{
    public class ZipCommand : Component, IZipCommand
    {
        private string _zipFilePath;
        private string _locationToZipPath;
        private string _zipContextLocation;
        public void Initialize(string[] parameters)
        {
            if (parameters.Length != 2)
                throw new OrchardException(T("ZipCommand failed"), new ArgumentException("ZipCommand expected exactly 2 arguments", "parameters"));

            _zipFilePath = parameters[0];

            var parts = parameters[1].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                _zipContextLocation = parameters[1].Substring(0, parameters[1].Length - parts[parts.Length - 1].Length);
                if (_zipFilePath.StartsWith(_zipContextLocation))
                    _zipFilePath = _zipFilePath.Substring(_zipContextLocation.Length);
                _locationToZipPath = parts[parts.Length - 1];
            }
            else
            {
                _locationToZipPath = parameters[1];
            }
        }
        public string CommandText {
            get
            {
                var contextCommand = string.Empty;
                if (!string.IsNullOrEmpty(_zipContextLocation))
                {
                    contextCommand = string.Format("cd {0};", _zipContextLocation.TrimEnd('/'));
                }
                //-r recursive
                return string.Format("{0}sudo zip -r {1} {2}"
                    ,contextCommand
                    , _zipFilePath
                    , _locationToZipPath);
            }
        }
    }
}