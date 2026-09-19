using Microsoft.AspNetCore.Http.HttpResults;

namespace ChatPlatform.Server.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public User? Sender { get; set; }
        public Guid GroupId { get; set; }
        public Group? Group { get; set; }
        public string Text { get; set; }
        public DateTimeOffset SentAt { get; set; }

        public Message(Guid senderId, Guid groupId, string text)
        {
            Id = Guid.NewGuid();
            SenderId = senderId;
            GroupId = groupId;
            Text = text;
            SentAt = DateTimeOffset.UtcNow;
        }
    }
}
