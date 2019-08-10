using System.Windows;

namespace LightsPacketAction
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow _window;
        public void AppStartup(object sender, StartupEventArgs e)
        {
            _window = new MainWindow();
            _window.DataContext = new MainWindowViewModel();

            _window.Show();
        }
    }
}
