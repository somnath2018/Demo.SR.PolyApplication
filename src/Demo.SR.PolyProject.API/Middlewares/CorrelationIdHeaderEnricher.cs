using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Demo.SR.PolyProject.API.Middlewares
{
    public  class CorrelationIdHeaderEnricherMiddleware
    {
        private const string CorrelationIdKey = "X-Correlation-Id";
        private const string RequestIdKey = "X-Request-Id";
        private readonly RequestDelegate _next;

        public CorrelationIdHeaderEnricherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(CorrelationIdKey, out var correlationId))
            {
                correlationId = $"GEN-{Guid.NewGuid().ToString("N")}";
            }
            if(!context.Request.Headers.TryGetValue(RequestIdKey,out  var requestId))
            {
                requestId = context.TraceIdentifier;
            }
            context.Items[CorrelationIdKey] = correlationId.ToString();
            context.Items[RequestIdKey] = requestId.ToString();

            context.Response.Headers.Add(CorrelationIdKey, correlationId);
            context.Response.Headers.Add(RequestIdKey, requestId.ToString());

            var logger = context.RequestServices.GetRequiredService<ILogger<CorrelationIdHeaderEnricherMiddleware>>();
            using (logger.BeginScope("{@SessionId}", correlationId))
            {
                await this._next(context);
            }
        }
        //}
        //    public  IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        //    => app.Use(async (context, next) =>
        //    {
        //        if(!context.Request.Headers.TryGetValue(CorrelationIdKey, out var correlationId))
        //        {
        //            correlationId = $"GEN-{Guid.NewGuid().ToString("N")}";
        //        }
        //        context.Items[CorrelationIdKey] = correlationId.ToString();
        //        context.Response.Headers.Add(CorrelationIdKey, correlationId);

        //        var logger = context.RequestServices.GetRequiredService<ILogger<CorrelationIdHeaderEnricher>>();
        //        using (logger.BeginScope("{@SessionId}", correlationId))
        //        {
        //            await this._next(context);
        //        }
        //        await next();

        //    });

       // public static string GetCorrelationId(this HttpContext context)
       //=> context.Items.TryGetValue(CorrelationIdKey, out var correlationId) ? correlationId as string : null;

       // public static void AddCorrelationId(this HttpRequestHeaders headers, string correlationId)
       //     => headers.TryAddWithoutValidation(CorrelationIdKey, correlationId);

    }

    
}
