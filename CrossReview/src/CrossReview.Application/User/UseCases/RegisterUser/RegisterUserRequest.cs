namespace CrossReview.Application.User.UseCases.Register;

public record RegisterUserRequest(string FirstName, string LastName, string Email, string Password, string PhoneNumber);