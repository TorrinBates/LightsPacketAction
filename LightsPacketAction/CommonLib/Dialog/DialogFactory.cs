using System.Windows;

namespace LightsPacketAction {
    public static class DialogFactory {

        public static void CreateErrorDialog(string title, string errorMessage, Window owner = null) {
            CustomWindow window = null;
            window = new CustomWindow(new ErrorViewModel(errorMessage, () => window.Close()), "Error");
            window.Owner = owner ?? Application.Current.MainWindow;
            window.MinimizeVisibility = Visibility.Collapsed;
            window.XVisibility = Visibility.Collapsed;
            window.MaxWidth = 300;
            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.ShowDialog();
        }

        public static bool CreateYesNoDialog(string title, string message) {
            CustomWindow window = null;
            var vm = new YesNoViewModel(message, () => window.Close());
            window = new CustomWindow(vm, title);
            window.Owner = Application.Current.MainWindow;
            window.MinimizeVisibility = Visibility.Collapsed;
            window.XVisibility = Visibility.Collapsed;
            window.MaxWidth = 300;
            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.ShowDialog();

            return vm.Result;
        }
    }
}
