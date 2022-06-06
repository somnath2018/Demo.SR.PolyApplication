using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Demo.SR.PolyProject.API.Utilitities
{
    public class CorrelationIdAccessor : ICorrelationIdAccessor
    {
        private readonly ILogger<CorrelationIdAccessor> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string CorrelationIdKey = "X-Correlation-Id";

        public CorrelationIdAccessor(ILogger<CorrelationIdAccessor> logger, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }

        public string GetSessionId()
        {
            try
            {
                var context = this._httpContextAccessor.HttpContext;
                var result = context?.Items[CorrelationIdKey] as string;

                return result;
            }
            catch (Exception exception)
            {
                this._logger.LogWarning(exception, "Unable to get original session id header");
            }

            return string.Empty;
        }
    }
}
