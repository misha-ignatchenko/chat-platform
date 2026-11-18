namespace ChatPlatform.Client.Services
{
    /// <summary>
    /// Сервер відхилив виклик методу Hub (наприклад, користувач не є учасником групи).
    /// Message містить лише текст сервера, без службового префікса SignalR.
    /// </summary>
    public class ChatHubException : Exception
    {
        public ChatHubException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
