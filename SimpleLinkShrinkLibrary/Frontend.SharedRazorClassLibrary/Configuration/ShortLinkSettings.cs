namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Configuration
{
    public class ShortlinkSettings
    {
        public int AliasLength { get; set; } = 5;
        public TimeSpan ExpirationSpan { get; set; } = TimeSpan.FromDays(10);
    }
}
