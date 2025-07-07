namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Exceptions
{
    public class ShortlinkDbConnectionStringNotFoundException : Exception
    {
        public ShortlinkDbConnectionStringNotFoundException() { }
        public ShortlinkDbConnectionStringNotFoundException(string message) : base(message) { }
        public ShortlinkDbConnectionStringNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
