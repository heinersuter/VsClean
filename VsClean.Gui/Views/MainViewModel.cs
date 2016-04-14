using System.Collections.Generic;
using System.IO;
using System.Linq;

using Alsolos.Commons.Wpf.Mvvm;

using Ninject;

using VsClean.Gui.Services;

namespace VsClean.Gui.Views
{
    public class MainViewModel : ViewModel
    {
        private readonly DirectoryService _directoryService;

        public MainViewModel(IKernel kernel)
        {
            var settingsService = kernel.Get<SettingsService>();
            settingsService.BeforeSave = () => settingsService.Config.RootDirectory = RootDirectory;
            RootDirectory = settingsService.Config.RootDirectory;

            _directoryService = kernel.Get<DirectoryService>();
        }

        public string RootDirectory
        {
            get { return BackingFields.GetValue<string>(); }
            set { BackingFields.SetValue(value, directory => FindCommand.RaiseCanExecuteChanged()); }
        }

        public List<DirectoryViewModel> Directories
        {
            get { return BackingFields.GetValue<List<DirectoryViewModel>>(); }
            set { BackingFields.SetValue(value); }
        }

        public DelegateCommand FindCommand => BackingFields.GetCommand(Find, CanFind);

        public DelegateCommand DeleteCommand => BackingFields.GetCommand(Delete, CanDelete);

        private bool CanFind()
        {
            return Directory.Exists(RootDirectory);
        }

        private void Find()
        {
            var directories = _directoryService.FindDirectoriesToDelete(RootDirectory);
            Directories = directories.Select(direcotry => new DirectoryViewModel(direcotry)).ToList();
        }

        private bool CanDelete()
        {
            return Directories != null && Directories.Any(viewModel => viewModel.IsSelected);
        }

        private void Delete()
        {
            _directoryService.Delete(Directories.Where(viewModel => viewModel.IsSelected).Select(viewModel => viewModel.Directory));
        }
    }
}
