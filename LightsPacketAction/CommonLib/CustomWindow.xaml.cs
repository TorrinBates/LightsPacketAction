using System.Windows;
using System.Windows.Input;

namespace LightsPacketAction
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CustomWindow : Window
    {
        public CustomWindow(ViewModelBase vm)
        {
            WindowContentViewModel = vm;
            DataContext = this;

            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));

            InitializeComponent();

            CenterWindowOnScreen();
        }

        public ViewModelBase WindowContentViewModel { get; private set; }

        public Visibility MinimizeVisibility { get; set; } = Visibility.Visible;

        public void CenterWindowOnScreen() {
            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);
            Top = (SystemParameters.PrimaryScreenHeight / 2) - (Height / 2);
        }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }
    }
}
