namespace CrossReview.Application.Project.DTOs;

public record ProjectListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Status { get; set; }

    public int MembersCount { get; set; }
    public int ActiveReviewPeriods { get; set; }
}