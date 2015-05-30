using System;

namespace ceenq.org.Services.Setting
{
    public interface IOutputSetting : IDefaultOptimization
    {
        /// <summary>
        ///     Timeout in seconds
        /// </summary>
        Int32 Timeout { get; set; }
    }
}