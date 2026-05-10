using CrossReview.Application;
using CrossReview.Infrastructure.Postgres;

namespace CrossReview;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure();
        
        return services;
    }

    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddOpenApi();
        
        return services;
    }
}