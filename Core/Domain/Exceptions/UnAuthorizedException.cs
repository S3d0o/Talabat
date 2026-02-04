namespace Domain.Exceptions
{
    public sealed class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(string message = $"Invalid Email or Password") : base(message)
        {
            
        }
    }
}
