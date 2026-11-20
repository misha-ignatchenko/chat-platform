using System.Windows;
using ChatPlatform.Client.Services;

namespace ChatPlatform.Client
{
    public partial class App : Application
    {
        private const string ServerUrl = "https://localhost:7004";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var api = new ApiClient(ServerUrl);
            var loginWindow = new LoginWindow(api);

            if (loginWindow.ShowDialog() != true)
            {
                Shutdown();
                return;
            }

            var mainWindow = new MainWindow(api);
            MainWindow = mainWindow;

            ShutdownMode = ShutdownMode.OnMainWindowClose;
            mainWindow.Show();
        }
    }
}