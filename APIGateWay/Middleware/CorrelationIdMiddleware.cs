using Serilog.Context;

namespace APIGateWay.Middleware
{
    public  class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeader="x-correlation-id";
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next,ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if(!httpContext.Request.Headers.TryGetValue(CorrelationIdHeader,out var correlationid))
            {
                correlationid=Guid.NewGuid().ToString();
                httpContext.Request.Headers[CorrelationIdHeader]=correlationid;
            }
            httpContext.Response.Headers[CorrelationIdHeader] = correlationid;

            // log visibility
            using(LogContext.PushProperty("correlationid", correlationid))
            {
                _logger.LogInformation("Correlation id set for{correlationid}", correlationid);
                await _next(httpContext);
            }
        }
    }
}
