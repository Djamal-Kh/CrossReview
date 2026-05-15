using CrossReview.Application.Project;
using CrossReview.Application.Review;
using CrossReview.Application.Template;
using CrossReview.Application.User;
using CrossReview.Infrastructure.Postgres.Repositories;
using CrossReview.Infrastructure.Postgres.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrossReview.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CrossReview");
        
        services.AddDbContext<CrossReviewDbContext>(options => 
            options.UseNpgsql(connectionString));

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IEvaluationResultRepository, EvaluationResultRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();

        services.AddScoped<DatabaseSeeder>();
            
        return services;
    }
}