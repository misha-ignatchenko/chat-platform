using System.Collections.Concurrent;
using ChatPlatform.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatPlatform.Client.Services
{
    public class ChatHubClient : IAsyncDisposable
    {
        private readonly HubConnection _connection;
        private readonly ConcurrentDictionary<Guid, bool> _joinedGroups = new();

        public event Action<ChatMessage>? MessageReceived;

        public ChatHubClient(ApiClient api)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($"{api.BaseUrl}/chathub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(api.Token);
                })
                .WithAutomaticReconnect().Build();

            _connection.On<ChatMessage>("ReceiveMessage", message => MessageReceived?.Invoke(message));

            _connection.Reconnected += _ => RejoinGroupsAsync();
        }

        public Task StartAsync() => _connection.StartAsync();

        public async Task JoinGroupAsync(Guid groupId)
        {
            try
            {
                await _connection.InvokeAsync("JoinGroup", groupId);
                _joinedGroups[groupId] = true;
            }
            catch (HubException ex)
            {
                throw new ChatHubException(CleanMessage(ex.Message), ex);
            }
        }

        public async Task SendMessageAsync(Guid groupId, string text)
        {
            try
            {
                await _connection.InvokeAsync("SendMessage", groupId, text);
            }
            catch (HubException ex)
            {
                throw new ChatHubException(CleanMessage(ex.Message), ex);
            }
        }

        private async Task RejoinGroupsAsync()
        {
            foreach (var groupId in _joinedGroups.Keys)
                await _connection.InvokeAsync("JoinGroup", groupId);
        }

        private static string CleanMessage(string raw)
        {
            const string marker = "HubException: ";
            var index = raw.IndexOf(marker, StringComparison.Ordinal);
            return index >= 0 ? raw[(index + marker.Length)..] : raw;
        }

        public ValueTask DisposeAsync() => _connection.DisposeAsync();
    }
}
