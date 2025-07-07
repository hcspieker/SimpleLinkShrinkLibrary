using Microsoft.AspNetCore.Http;

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri GetBaseUrl(this HttpRequest request)
        {
            return new Uri($"{request.Scheme}{Uri.SchemeDelimiter}{request.Host}{request.PathBase}");
        }
    }
}
