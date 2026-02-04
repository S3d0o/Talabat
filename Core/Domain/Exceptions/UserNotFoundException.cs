namespace Domain.Exceptions
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string email) : base($"User with email '{email}' was not found.")
        {
        }
    }
}
