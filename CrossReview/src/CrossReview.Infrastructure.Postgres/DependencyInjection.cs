using CrossReview.Application.Project;
using CrossReview.Application.Review;
using CrossReview.Application.Template;
using CrossReview.Application.User;
using CrossReview.Infrastructure.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CrossReview.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<CrossReviewDbContext>();

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IEvaluationResultRepository, EvaluationResultRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}