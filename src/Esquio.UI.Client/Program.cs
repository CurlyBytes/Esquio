﻿using Esquio.UI.Client.Infrastructure.Authorization;
using Esquio.UI.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Esquio.UI.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await ConfigureHost(args).Build().RunAsync();
        }

        static WebAssemblyHostBuilder ConfigureHost(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents
                .Add<App>("app");

            builder.Logging
                .SetMinimumLevel(LogLevel.Warning);

            builder.Services.AddHttpClient<IEsquioHttpClient, EsquioHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Security", options.ProviderOptions);

                var scope = builder.Configuration
                    .GetValue<string>("Security:Client:Scope");

                options.ProviderOptions.DefaultScopes.Add(scope);
            });

            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(Policies.Reader, builder => builder.AddRequirements(new PolicyRequirement(Policies.Reader)));
                options.AddPolicy(Policies.Contributor, builder => builder.AddRequirements(new PolicyRequirement(Policies.Contributor)));
                options.AddPolicy(Policies.Management, builder => builder.AddRequirements(new PolicyRequirement(Policies.Management)));
            });

            builder.Services.AddScoped<EsquioState>();
            builder.Services.AddScoped<ConfirmState>();
            builder.Services.AddScoped<ILocalStorage, LocalStorage>();
            builder.Services.AddScoped<IPolicyBuilder, DefaultPolicyBuilder>();
            builder.Services.AddScoped<INotifications, Notifications>();
            builder.Services.AddScoped<IAuthorizationHandler, PolicyRequirementHandler>();

            return builder;
        }
    }
}
