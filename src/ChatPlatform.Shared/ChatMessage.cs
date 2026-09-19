namespace ChatPlatform.Shared;

public record ChatMessage(Guid Id, Guid SenderId, Guid RoomId, string Text, DateTimeOffset SentAt);