namespace SimpleLinkShrinkLibrary.Core.Application.Configuration
{
    public interface IShortlinkDefaultValues
    {
        int AliasLength { get; }
        TimeSpan ExpirationSpan { get; }
    }
}
