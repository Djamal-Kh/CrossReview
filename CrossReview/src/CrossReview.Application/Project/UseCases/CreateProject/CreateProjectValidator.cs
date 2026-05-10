using FluentValidation;

namespace CrossReview.Application.Project.UseCases.CreateProject;

public class CreateProjectValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        
    }
}