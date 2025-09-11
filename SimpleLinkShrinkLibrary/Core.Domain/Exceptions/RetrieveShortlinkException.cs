using System.Runtime.Serialization;

namespace SimpleLinkShrinkLibrary.Core.Domain.Exceptions
{

    [Serializable]
    public class RetrieveShortlinkException : Exception
    {
        public RetrieveShortlinkException() { }
        public RetrieveShortlinkException(string message) : base(message) { }
        public RetrieveShortlinkException(string message, Exception inner) : base(message, inner) { }
        protected RetrieveShortlinkException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
