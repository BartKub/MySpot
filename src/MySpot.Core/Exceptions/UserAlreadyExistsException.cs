namespace MySpot.Core.Exceptions;

public sealed class UserAlreadyExistsException : CustomException
{
    public string Email { get; }
    public UserAlreadyExistsException(string email)
        : base($"User with {email} already exists")
    {
        Email = email;
    }
}