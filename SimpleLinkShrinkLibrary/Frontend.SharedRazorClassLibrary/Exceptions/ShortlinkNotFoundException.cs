namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Exceptions
{
    public class ShortlinkNotFoundException : Exception
    {
        public ShortlinkNotFoundException() { }
        public ShortlinkNotFoundException(string message) : base(message) { }
        public ShortlinkNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
