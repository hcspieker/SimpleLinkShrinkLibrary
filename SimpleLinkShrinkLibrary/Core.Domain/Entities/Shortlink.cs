
namespace SimpleLinkShrinkLibrary.Core.Domain.Entities
{
    public class Shortlink : EntityBase
    {
        public required string TargetUrl { get; set; }
        public required string Alias { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
