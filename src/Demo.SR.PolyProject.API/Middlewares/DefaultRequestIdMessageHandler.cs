using Demo.SR.PolyProject.API.Utilitities;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.SR.PolyProject.API.Middlewares
{
    public class DefaultRequestIdMessageHandler : DelegatingHandler
    {
        private readonly ICorrelationIdAccessor _correlationIdAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultRequestIdMessageHandler(ICorrelationIdAccessor correlationIdAccessor,
            IHttpContextAccessor httpContextAccessor)
        {
            this._correlationIdAccessor = correlationIdAccessor;
            this._httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Request-Id", _httpContextAccessor.HttpContext.TraceIdentifier);
            request.Headers.Add("X-SessionId", _correlationIdAccessor.GetSessionId());

            return base.SendAsync(request, cancellationToken);
        }
    }
}
