namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Models
{
    public class ShortlinkDetailViewModel
    {
        public int Id { get; set; }
        public string TargetUrl { get; set; }
        public string ShortlinkUrl { get; set; }
        public string StatusUrl { get;  set; }
        public DateTime? ExpirationDate { get; set; }

        public ShortlinkDetailViewModel()
        {
            TargetUrl = string.Empty;
            ShortlinkUrl = string.Empty;
            StatusUrl = string.Empty;
        }
    }
}
