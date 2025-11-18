using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Attributes.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Models
{
    public class ShortlinkCreateViewModel
    {
        [Required]
        [HttpUrl]
        [DisplayName("Link")]
        public string? TargetUrl { get; set; }
    }
}
