namespace CrossReview.Domain.User;

public class User
{
    public User(
        Guid id, 
        string firstName,
        string lastName,
        string email, 
        string phoneNumber, 
        EnumUserRole role = EnumUserRole.User)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
    }
    
    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    public EnumUserRole Role { get; }
}