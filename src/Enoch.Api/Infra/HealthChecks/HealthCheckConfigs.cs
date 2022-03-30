using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Mime;
using System.Text.Json;

namespace Enoch.Api.Infra.HealthChecks
{
    public static class HealthCheckConfigs
    {
        public static void ApiHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health",
            new HealthCheckOptions()
            {
                Predicate = p => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options => { options.UIPath = "/dashboard"; });
        }

        public static void ConfigureHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddHealthChecksUI(options =>
            {
                options.SetEvaluationTimeInSeconds(5);
                options.MaximumHistoryEntriesPerEndpoint(10);
                options.AddHealthCheckEndpoint("Enoch API", "/health");
            })
            .AddInMemoryStorage();
        }
    }
}
