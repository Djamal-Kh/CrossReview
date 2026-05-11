using CrossReview.Application.Project.UseCases.AddNewReviewPeriod;
using CrossReview.Application.Project.UseCases.ArchiveReviewPeriod;
using CrossReview.Application.Project.UseCases.AssignNewProjectMember;
using CrossReview.Application.Project.UseCases.ChangeProjectMemberRole;
using CrossReview.Application.Project.UseCases.CloseProject;
using CrossReview.Application.Project.UseCases.CloseReviewPeriod;
using CrossReview.Application.Project.UseCases.CreateProject;
using CrossReview.Application.Project.UseCases.DeactivateProjectMember;
using CrossReview.Application.Project.UseCases.DeleteProject_maybeDelete;
using CrossReview.Application.Project.UseCases.GetProjectById;
using CrossReview.Application.Project.UseCases.GetProjectMemberById;
using CrossReview.Application.Project.UseCases.GetProjectMembers;
using CrossReview.Application.Project.UseCases.GetProjects;
using CrossReview.Application.Project.UseCases.RemoveProjectMember;
using CrossReview.Application.Project.UseCases.StartProject;
using CrossReview.Application.Project.UseCases.UpdateProjectData;
using CrossReview.Application.Project.UseCases.UpdateProjectDescription;
using CrossReview.Application.Project.UseCases.UpdateProjectTitle;
using CrossReview.Application.Project.UseCases.UpdateReviewPeriodDates;
using CrossReview.Application.Review.UseCases.AddAnswerToReview;
using CrossReview.Application.Review.UseCases.CalculateEvaluationResult;
using CrossReview.Application.Review.UseCases.CloseAllReviewsForPeriod;
using CrossReview.Application.Review.UseCases.CloseReview;
using CrossReview.Application.Review.UseCases.CreateReview;
using CrossReview.Application.Review.UseCases.GenerateReviewsForPeriod;
using CrossReview.Application.Review.UseCases.GetEvaluationResult;
using CrossReview.Application.Review.UseCases.GetReviewByParameters;
using CrossReview.Application.Review.UseCases.GetReviewsForProjectAndPeriod;
using CrossReview.Application.Review.UseCases.GetReviewsForUser;
using CrossReview.Application.Review.UseCases.RecalculateEvaluationResult;
using CrossReview.Application.Review.UseCases.RemoveAnswerFromReview;
using CrossReview.Application.Review.UseCases.SubmitReview;
using CrossReview.Application.Review.UseCases.UpdateAnswerInReview;
using CrossReview.Application.Template.UseCases.ActivateTemplate;
using CrossReview.Application.Template.UseCases.AddQuestionToTemplate;
using CrossReview.Application.Template.UseCases.CreateTemplate;
using CrossReview.Application.Template.UseCases.DeactivateTemplate;
using CrossReview.Application.Template.UseCases.DeleteTemplate;
using CrossReview.Application.Template.UseCases.RemoveQuestionFromTemplate;
using CrossReview.Application.Template.UseCases.ReorderQuestions;
using CrossReview.Application.Template.UseCases.UpdateQuestionInTemplate;
using CrossReview.Application.Template.UseCases.UpdateTemplateTitle;
using CrossReview.Application.User.UseCases.DeleteUser;
using CrossReview.Application.User.UseCases.GetUserByEmail;
using CrossReview.Application.User.UseCases.GetUserById;
using CrossReview.Application.User.UseCases.Login;
using CrossReview.Application.User.UseCases.Register;
using CrossReview.Application.User.UseCases.RegisterAdmin;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CrossReview.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Project
        services.AddScoped<AddNewPeriodUseCase>();
        services.AddScoped<ArchiveReviewPeriodUseCase>();
        services.AddScoped<AssignNewProjectMemberUseCase>();
        services.AddScoped<ChangeProjectMemberRoleUseCase>();
        services.AddScoped<CloseProjectUseCase>();
        services.AddScoped<CloseReviewPeriodUseCase>();
        services.AddScoped<CreateProjectUseCase>();
        services.AddScoped<DeactivateProjectMemberUseCase>();
        services.AddScoped<DeleteProjectUseCase>();
        services.AddScoped<GetProjectByIdUseCase>();
        services.AddScoped<GetProjectMemberByIdUseCase>();
        services.AddScoped<GetProjectMembersUseCase>();
        services.AddScoped<GetProjectsUseCase>();
        services.AddScoped<RemoveProjectMemberUseCase>();
        services.AddScoped<StartProjectUseCase>();
        services.AddScoped<UpdateProjectUseCase>();
        services.AddScoped<UpdateProjectDescriptionUseCase>();
        services.AddScoped<UpdateProjectTitleUseCase>();
        services.AddScoped<UpdateReviewPeriodDatesUseCase>();
        
        
        // Review
        services.AddScoped<AddAnswerUseCase>();
        services.AddScoped<CloseReviewPeriodUseCase>(); // недоделанный
        services.AddScoped<CalculateEvaluationResultUseCase>();
        services.AddScoped<CloseAllReviewsForPeriodUseCase>(); // недоделанный
        services.AddScoped<CloseReviewUseCase>();
        services.AddScoped<CreateReviewUseCase>();
        services.AddScoped<GenerateReviewsForPeriodUseCase>(); // недоделанный
        services.AddScoped<GetEvaluationResultUseCase>();
        services.AddScoped<GetReviewByParametersUseCase>();
        services.AddScoped<GetProjectReviewsUseCase>();
        services.AddScoped<GetReviewsForUserUseCase>();
        services.AddScoped<RecalculateEvaluationResultUseCase>();
        services.AddScoped<RemoveAnswerUseCase>();
        services.AddScoped<SubmitReviewUseCase>();
        services.AddScoped<UpdateAnswerUseCase>();
        
        
        // Template
        services.AddScoped<ActivateTemplateUseCase>();
        services.AddScoped<AddQuestionUseCase>();
        services.AddScoped<CreateTemplateUseCase>();
        services.AddScoped<DeactivateTemplateUseCase>();
        services.AddScoped<DeleteTemplateUseCase>();
        services.AddScoped<RemoveQuestionUseCase>();
        services.AddScoped<ReorderQuestionsUseCase>();
        services.AddScoped<UpdateQuestionUseCase>();
        services.AddScoped<UpdateTemplateTitleUseCase>();
        services.AddScoped<RegisterUserUseCase>();
        services.AddScoped<DeleteUserUseCase>();
        services.AddScoped<GetUserByEmailUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        
        
        // User
        services.AddScoped<DeleteUserUseCase>();
        services.AddScoped<GetUserByEmailUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterUserUseCase>();
        services.AddScoped<RegisterAdminUseCase>();
        
        return services;
    }
}