namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.UnitTests.Exceptions
{

    [Serializable]
    public class DummyException : Exception
    {
        public DummyException() { }
        public DummyException(string message) : base(message) { }
        public DummyException(string message, Exception inner) : base(message, inner) { }
    }
}
