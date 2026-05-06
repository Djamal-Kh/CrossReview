using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.User;

public class UserEntity
{
    private UserEntity(
        Guid id, 
        string firstName,
        string lastName,
        string email, 
        string password,
        string phoneNumber,
        EnumUserRole role
        )
    {
        Validate(firstName, lastName, email, phoneNumber);
        
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        Role = role;
    }
    
    public Guid Id { get; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Password {get; private set;}
    public EnumUserRole Role { get; private set; }

    public static UserEntity Create(string firstName,
        string lastName,
        string password,
        string email,
        string phoneNumber,
        EnumUserRole role)
    {
        return new UserEntity(
            Guid.NewGuid(),
            firstName,
            lastName,
            email,
            password, phoneNumber, role);
    }
    
    public void ChangeRole(EnumUserRole newRole)
    {
        if (Role == newRole)
            return;

        Role = newRole;
    }

    public void UpdateProfile(string firstName, string lastName, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ValidationException($"Поле {nameof(FirstName)} не может быть пустым");
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException($"Поле {nameof(LastName)} не может быть пустым");
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ValidationException($"Поле {nameof(PhoneNumber)} не может быть пустым");
        
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public void UpdateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException($"Поле {nameof(Email)} не может быть пустым");
        
        Email = email;
    }

    private void Validate(
        string firstName,
        string lastName,
        string email, 
        string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ValidationException($"Поле {nameof(FirstName)} не может быть пустым");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException($"Поле {nameof(LastName)} не может быть пустым");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ValidationException($"Поле {nameof(PhoneNumber)} не может быть пустым");

        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException($"Поле {nameof(Email)} не может быть пустым");
    }
}