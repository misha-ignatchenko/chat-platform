namespace ChatPlatform.Shared;

public record ChatMessage(Guid Id, Guid SenderId, Guid GroupId, string Text, DateTimeOffset SentAt);