namespace ChatPlatform.Server.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public User(string login, string passwordHash)
        {
            Id = Guid.NewGuid();
            Login = login;
            PasswordHash = passwordHash;
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
