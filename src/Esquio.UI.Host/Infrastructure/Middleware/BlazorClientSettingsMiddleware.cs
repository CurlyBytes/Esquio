using Esquio.UI.Host.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Esquio.UI.Host.Infrastructure.Middleware
{
    internal class BlazorClientConfigurationMiddleware
    {
        private readonly RequestDelegate _next;

        public BlazorClientConfigurationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            if (context.Request.Path.StartsWithSegments(new PathString("/appsettings.json")))
            {
                var securityOptions = new SetttingsResponse();
                securityOptions.Security = configuration.GetSection<Security>().Client;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(securityOptions));

                return;
            }

            await _next(context);
        }

        private class SetttingsResponse
        {
            public Options.Client Security { get; set; } = new Options.Client();
        }
    }
}
