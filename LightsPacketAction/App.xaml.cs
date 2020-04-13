using System.Windows;

namespace LightsPacketAction
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        CustomWindow _window;
        ConfigHandler _configHandler;
        public void AppStartup(object sender, StartupEventArgs e)
        {
            _configHandler = new ConfigHandler();

            //The showing of error dialogs must be done after the main window shows since dialogs set the owner to the main window which wouldn't have been created yet
            var rc = _configHandler.LoadConfig();

            var vm = new LauncherViewModel(_configHandler);
            _window = new CustomWindow(vm);
            _window.Show();

            if (rc.ReturnCode != ConfigHandlerReturnCodeType.Success) {
                _configHandler.CreateNewConfig();
                if (rc.ReturnCode == ConfigHandlerReturnCodeType.InvalidConfiguration) {
                    var result = DialogFactory.CreateYesNoDialog(
                        Constants.C_ConfigErrorTitle,
                        "Could not load configuration file due to it being invalid.\n\nUsing a new configuration for now. Would you like to overwrite the new configuration with the new configuration?"
                    );
                    if (result) AttemptToSaveConfig();
                } else if (rc.ReturnCode == ConfigHandlerReturnCodeType.FileNotFound)
                    AttemptToSaveConfig();
                else DialogFactory.CreateErrorDialog(Constants.C_ConfigErrorTitle, rc.Reason);
            }
        }

        private void AttemptToSaveConfig() {
            var saveRc = _configHandler.SaveConfig();
            if (saveRc.ReturnCode != ConfigHandlerReturnCodeType.Success) DialogFactory.CreateErrorDialog(Constants.C_ConfigErrorTitle, saveRc.Reason);
        }
    }
}
