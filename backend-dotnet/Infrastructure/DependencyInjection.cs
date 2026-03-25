using backend_dotnet.Interfaces;
using backend_dotnet.Services;
using Microsoft.OpenApi.Models;

namespace backend_dotnet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services)
    {
        services.AddSingleton<IHealthService, HealthService>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(static o =>
        {
            o.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "AI Web3 Agent Gateway",
                    Version = "v1",
                    Description = ".NET API gateway and orchestrator.",
                });
        });
        return services;
    }

    public static WebApplication UseGatewayPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (ListensForHttps(app.Configuration))
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();
        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        }

        return app;
    }

    private static bool ListensForHttps(IConfiguration configuration)
    {
        var urls =
            configuration["urls"]
            ?? configuration["Urls"]
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
            ?? string.Empty;
        return urls.Contains("https://", StringComparison.OrdinalIgnoreCase);
    }
}
