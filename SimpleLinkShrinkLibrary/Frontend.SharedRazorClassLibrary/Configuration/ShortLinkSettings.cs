namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Configuration
{
    public class ShortLinkSettings
    {
        public int LinkAliasLength { get; set; } = 5;
        public TimeSpan LinkExpirationSpan { get; set; } = TimeSpan.FromDays(10);
    }
}
