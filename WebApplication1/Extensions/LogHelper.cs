using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public static class LogHelper
    {
        public static void EnrichFromRequest(this IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            diagnosticContext.Set("Host", httpContext.Request.Host);
            diagnosticContext.Set("Protocol", httpContext.Request.Protocol);
            diagnosticContext.Set("Scheme", httpContext.Request.Scheme);

            if (httpContext.Request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", httpContext.Request.QueryString);
            }

            diagnosticContext.Set("ContentType", httpContext.Request.ContentType);

            var endpoint = httpContext.GetEndpoint();
            if (endpoint != null)
            {
                diagnosticContext.Set("Endpoint", endpoint.DisplayName);
            }

        }
    }
}
