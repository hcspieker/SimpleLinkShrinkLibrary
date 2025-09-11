using System.Runtime.Serialization;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.Exceptions
{

    [Serializable]
    public class ConnectionStringNotFoundException : Exception
    {
        public ConnectionStringNotFoundException() { }
        public ConnectionStringNotFoundException(string message) : base(message) { }
        public ConnectionStringNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ConnectionStringNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
