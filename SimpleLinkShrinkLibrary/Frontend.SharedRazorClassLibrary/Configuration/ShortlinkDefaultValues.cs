using Microsoft.Extensions.Options;
using SimpleLinkShrinkLibrary.Core.Application.Configuration;

namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Configuration
{
    internal class ShortlinkDefaultValues(IOptions<ShortlinkSettings> options) : IShortlinkDefaultValues
    {
        public int AliasLength => options.Value.AliasLength;
        public TimeSpan ExpirationSpan => options.Value.ExpirationSpan;
    }
}
