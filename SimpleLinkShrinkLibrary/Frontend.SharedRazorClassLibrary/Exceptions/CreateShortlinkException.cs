namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Exceptions
{
    public class CreateShortlinkException : Exception
    {
        public CreateShortlinkException() { }
        public CreateShortlinkException(string message) : base(message) { }
        public CreateShortlinkException(string message, Exception inner) : base(message, inner) { }
    }
}
