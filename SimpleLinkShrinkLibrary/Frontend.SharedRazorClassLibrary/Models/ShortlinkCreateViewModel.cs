using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Models
{
    public class ShortlinkCreateViewModel
    {
        [Required]
        [Url(ErrorMessage = "The Link field is not a valid fully-qualified http, or https URL.")]
        [DisplayName("Link")]
        public string? TargetUrl { get; set; }
    }
}
