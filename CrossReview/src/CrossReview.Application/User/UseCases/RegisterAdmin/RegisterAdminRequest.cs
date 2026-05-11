namespace CrossReview.Application.User.UseCases.RegisterAdmin;

public record RegisterAdminRequest(string FirstName, string LastName, string Email, string Password, string PhoneNumber);