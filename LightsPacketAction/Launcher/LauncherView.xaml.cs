using System.Windows.Controls;

namespace LightsPacketAction
{
    /// <summary>
    /// Interaction logic for LauncherView.xaml
    /// </summary>
    public partial class LauncherView : UserControl
    {
        public LauncherView()
        {
            DataContext = new LauncherViewModel();
            InitializeComponent();
        }
    }
}
