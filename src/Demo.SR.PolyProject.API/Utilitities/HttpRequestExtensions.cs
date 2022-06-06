using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Demo.SR.PolyProject.API.Utilitities
{
    public static class HttpRequestExtensions
    {
        public static string GetRequestHeaderOrDefault(this HttpRequest request, string key, string deafultValue = null)
        {
            return request?.Headers?.FirstOrDefault(_ => _.Key.Equals(key)).Value.FirstOrDefault() ?? deafultValue;
        }
    }
}
