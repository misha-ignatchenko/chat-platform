using System.Windows;
using ChatPlatform.Client.Services;

namespace ChatPlatform.Client
{
    public partial class MainWindow : Window
    {
        private readonly ApiClient _api;

        public MainWindow(ApiClient api)
        {
            InitializeComponent();
            _api = api;
        }
    }
}
