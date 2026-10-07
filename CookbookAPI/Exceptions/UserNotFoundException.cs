namespace CookbookAPI.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public int Id { get; }
        public UserNotFoundException(int userId) : base($"User id - {userId}, not found.")
        {
            Id = userId;
        }
    }
}
