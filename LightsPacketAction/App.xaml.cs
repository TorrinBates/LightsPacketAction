using System.Windows;

namespace LightsPacketAction
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private CustomWindow _window;
        public void AppStartup(object sender, StartupEventArgs e)
        {
            _window = new CustomWindow(new LauncherViewModel());
            _window.Show();
        }
    }
}
