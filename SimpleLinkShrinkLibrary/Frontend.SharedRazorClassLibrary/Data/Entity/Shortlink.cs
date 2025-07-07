using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data.Entity
{
    [Index(nameof(Alias), IsUnique = true)]
    public class Shortlink
    {
        [Key]
        public int Id { get; set; }
        public required string TargetUrl { get; set; }
        public required string Alias { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
