namespace ChatPlatform.Server.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Group(string name, Guid ownerId)
        {
            Id = Guid.NewGuid();
            Name = name;
            OwnerId = ownerId;
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
