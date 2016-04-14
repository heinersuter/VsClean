using System.Windows;

using Ninject;

using VsClean.Gui.Services;
using VsClean.Gui.Views;

namespace VsClean.Gui
{
    public partial class App
    {
        private StandardKernel _kernel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _kernel = new StandardKernel();
            _kernel.Bind<SettingsService>().ToSelf().InSingletonScope();

            var mainWindowViewModel = new MainWindowViewModel(_kernel);
            var mainWindow = new MainWindow { DataContext = mainWindowViewModel };
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var settingsService = _kernel.Get<SettingsService>();
            settingsService.Save();

            base.OnExit(e);
        }
    }
}
