using System;
using System.Runtime.Serialization;

namespace ceenq.org.Services
{
    [Serializable]
    public class EsvBibleException : Exception
    {
        public EsvBibleException()
        {
        }

        public EsvBibleException(String message) : base(message)
        {
        }

        public EsvBibleException(String message, Exception inner) : base(message, inner)
        {
        }

        protected EsvBibleException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}