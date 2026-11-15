using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ChatPlatform.Shared;

namespace ChatPlatform.Client.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;

        public ApiClient(string baseUrl)
        {
            BaseUrl = baseUrl;
            _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public string BaseUrl { get; }

        public string? Token { get; private set; }

        public async Task RegisterAsync(string login, string password)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/register", new { login, password });
            await EnsureSuccessAsync(response);
        }

        public async Task LoginAsync(string login, string password)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", new { login, password });
            await EnsureSuccessAsync(response);

            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            Token = result!.Token;

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }

        public async Task<List<GroupInfo>> GetGroupsAsync()
        {
            var response = await _http.GetAsync("/api/groups");
            await EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<List<GroupInfo>>() ?? new List<GroupInfo>();
        }

        public async Task<Guid> CreateGroupAsync(string name)
        {
            var response = await _http.PostAsJsonAsync("/api/groups", new { name });
            await EnsureSuccessAsync(response);

            var created = await response.Content.ReadFromJsonAsync<GroupCreated>();
            return created!.Id;
        }

        public async Task AddMemberAsync(Guid groupId, string login)
        {
            var response = await _http.PostAsJsonAsync($"/api/groups/{groupId}/members", new { login });
            await EnsureSuccessAsync(response);
        }

        public async Task<List<ChatMessage>> GetHistoryAsync(Guid groupId, DateTimeOffset? before = null, int limit = 50)
        {
            var url = $"/api/groups/{groupId}/messages?limit={limit}";

            if (before is not null)
                url += $"&before={Uri.EscapeDataString(before.Value.ToString("O"))}";

            var response = await _http.GetAsync(url);
            await EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<List<ChatMessage>>() ?? new List<ChatMessage>();
        }

        private static async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var body = await response.Content.ReadAsStringAsync();
            var message = string.IsNullOrWhiteSpace(body)
                ? response.ReasonPhrase ?? "Request failed"
                : body;

            throw new ApiException(response.StatusCode, message);
        }
    }

    internal record LoginResult(string Token);
    internal record GroupCreated(Guid Id);
}
