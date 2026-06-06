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
using CrossReview.Application.Review.UseCases.CreateReview;
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
using CrossReview.Application.User.UseCases.GetCurrentUser;
using CrossReview.Application.User.UseCases.GetUserByEmail;
using CrossReview.Application.User.UseCases.GetUserById;
using CrossReview.Application.User.UseCases.Login;
using CrossReview.Application.User.UseCases.RegisterAdmin;
using CrossReview.Application.User.UseCases.RegisterUser;
using CrossReview.Application.Project.UseCases.ActivateReviewPeriod;
using CrossReview.Application.Project.UseCases.GetProjectsByUserId;
using CrossReview.Application.User.UseCases.GetCurrentUser;
using CrossReview.Application.Template.UseCases.GetTemplateById;
using CrossReview.Application.Review.UseCases.CalculateAllResultsForPeriod;
using CrossReview.Application.Review.UseCases.GetAllEvaluatuinResults;
using CrossReview.Application.Review.UseCases.GetEvaluationResultsByProjectId;
using CrossReview.Application.Review.UseCases.GetEvaluationResulyByUserId;
using CrossReview.Application.Template.UseCases.GetAllTemplates;
using CrossReview.Application.User.UseCases.GetAllUsers;
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
        services.AddScoped<GetProjectsUseCase>();
        services.AddScoped<RemoveProjectMemberUseCase>();
        services.AddScoped<StartProjectUseCase>();
        services.AddScoped<UpdateProjectUseCase>();
        services.AddScoped<UpdateProjectDescriptionUseCase>();
        services.AddScoped<UpdateProjectTitleUseCase>();
        services.AddScoped<UpdateReviewPeriodDatesUseCase>();
        services.AddScoped<GetProjectMembersUseCase>();
        services.AddScoped<ActivateReviewPeriodUseCase>();
        services.AddScoped<GetProjectsByUserIdUseCase>();

        // Review
        services.AddScoped<AddAnswerUseCase>();
        services.AddScoped<CalculateEvaluationResultUseCase>();
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
        services.AddScoped<ClosePeriodReviewsUseCase>();
        services.AddScoped<GetAllEvaluationResultsUseCase>();
        services.AddScoped<GetEvaluationResultsByProjectIdUseCase>();
        services.AddScoped<GetEvaluationResultByUserIdUseCase>();
        

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
        services.AddScoped<GetTemplateByIdUseCase>();
        services.AddScoped<GetAllTemplateUseCase>();
        

        // User
        services.AddScoped<DeleteUserUseCase>();
        services.AddScoped<GetUserByEmailUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterUserUseCase>();
        services.AddScoped<RegisterAdminUseCase>();
        services.AddScoped<GetCurrentUserUseCase>();
        services.AddScoped<GetAllUsersUseCase>();

        return services;
    }
}