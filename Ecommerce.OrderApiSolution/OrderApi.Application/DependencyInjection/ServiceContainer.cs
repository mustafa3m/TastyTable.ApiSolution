using ecommerce.SharedLibrary.logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            // Registrerer HttpClient-tjenesten for å kommunisere med API Gateway
            // Setter base-adressen og timeout på 1 sekund
            services.AddHttpClient<IOrderService, OrderService>(options =>
            {
                options.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
               
                options.Timeout = TimeSpan.FromSeconds(1);
            });

            // Oppretter en retry-strategi for å håndtere feil automatisk
            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(), // Håndterer TaskCanceledException-feil
                BackoffType = DelayBackoffType.Constant, // Konstant ventetid mellom forsøk
                UseJitter = true, // Bruker jitter for å spre belastningen
                MaxRetryAttempts = 3, // Prøver på nytt maks 3 ganger
                Delay = TimeSpan.FromMilliseconds(500), // Venter 500 ms mellom hver retry
                OnRetry = args => // Logger hver gang en retry skjer
                {
                    string message = $"OnRetry, Attempt: {args.AttemptNumber}.Outcome {args.Outcome}";
                    LogException.LogToConsole(message); // Logger til konsollen
                    LogException.LogToDebugger(message); // Logger til debuggeren
                    return ValueTask.CompletedTask;
                }
            };

            // Legger til retry-strategien i en resilience pipeline for feilhåndtering
            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(retryStrategy); // Knytter retry-strategien til pipelinen
            });

            return services; // Returnerer den oppdaterte service collection
        }
    }
}

