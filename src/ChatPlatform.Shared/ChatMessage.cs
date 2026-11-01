namespace ChatPlatform.Shared;

public record ChatMessage(Guid Id, Guid SenderId, string SenderLogin, Guid GroupId, string Text, DateTimeOffset SentAt);