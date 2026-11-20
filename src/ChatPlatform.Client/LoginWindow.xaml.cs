using System.Net.Http;
using System.Windows;
using ChatPlatform.Client.Services;
using ChatPlatform.Shared;

namespace ChatPlatform.Client
{
    public partial class LoginWindow : Window
    {
        private readonly ApiClient _api;

        public LoginWindow(ApiClient api)
        {
            InitializeComponent();
            _api = api;

            LoginInput.MaxLength = UserLimits.MaxLoginLength;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetCredentials(out var login, out var password))
                return;

            await RunAsync(async () =>
            {
                await _api.LoginAsync(login, password);
                DialogResult = true;
            });
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetCredentials(out var login, out var password))
                return;

            await RunAsync(async () =>
            {
                await _api.RegisterAsync(login, password);

                await _api.LoginAsync(login, password);
                DialogResult = true;
            });
        }

        private bool TryGetCredentials(out string login, out string password)
        {
            login = LoginInput.Text.Trim();
            password = PasswordInput.Password;

            if (login.Length == 0 || password.Length == 0)
            {
                ErrorText.Text = "Enter your username and password.";
                return false;
            }

            return true;
        }

        private async Task RunAsync(Func<Task> action)
        {
            ErrorText.Text = string.Empty;
            SetBusy(true);

            try
            {
                await action();
            }
            catch (ApiException ex)
            {
                ErrorText.Text = ex.Message;
            }
            catch (HttpRequestException)
            {
                ErrorText.Text = "Unable to connect to the server. Check to see if it's running.";
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void SetBusy(bool busy)
        {
            LoginButton.IsEnabled = !busy;
            RegisterButton.IsEnabled = !busy;
        }
    }
}
