using System;

namespace ceenq.org.Services.Parameter
{
    public abstract class Parameter
    {
        private String key;

        /// <summary>
        ///     Your access key. For testing purposes, you can use the key TEST. For general-purpose queries, you can use the key IP.
        /// </summary>
        public String Key
        {
            get { return key; }
            set { key = value; }
        }
    }
}