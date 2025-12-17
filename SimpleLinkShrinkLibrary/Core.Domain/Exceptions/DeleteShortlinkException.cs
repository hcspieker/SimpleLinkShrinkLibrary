
namespace SimpleLinkShrinkLibrary.Core.Domain.Exceptions
{

    [Serializable]
    public class DeleteShortlinkException : Exception
    {
        public DeleteShortlinkException() { }
        public DeleteShortlinkException(string message) : base(message) { }
        public DeleteShortlinkException(string message, Exception inner) : base(message, inner) { }
    }
}
