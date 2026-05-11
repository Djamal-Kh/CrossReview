namespace CrossReview.Application.User.DTO;

public record UserIdentityResult()
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
}