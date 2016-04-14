using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public bool? AreAllSelected
        {
            get { return BackingFields.GetValue<bool?>(); }
            set { BackingFields.SetValue(value, SelectAll); }
        }

        public List<DirectoryViewModel> Directories
        {
            get { return BackingFields.GetValue<List<DirectoryViewModel>>(); }
            set { BackingFields.SetValue(value); }
        }

        public DelegateCommand FindCommand => BackingFields.GetCommand(Find, CanFind);

        public DelegateCommand DeleteCommand => BackingFields.GetCommand(Delete, CanDelete);

        private void SelectAll(bool? areAllSelected)
        {
            if (areAllSelected == null || Directories == null)
            {
                return;
            }
            Directories.ForEach(vm => vm.IsSelected = areAllSelected.Value);
        }

        private bool CanFind()
        {
            return Directory.Exists(RootDirectory);
        }

        private void Find()
        {
            Directories?.ForEach(vm => vm.PropertyChanged -= IsSelectedChanged);

            var directories = _directoryService.FindDirectoriesToDelete(RootDirectory);

            Directories = directories.Select(
                direcotry =>
                {
                    var viewModel = new DirectoryViewModel(direcotry);
                    viewModel.PropertyChanged += IsSelectedChanged;
                    return viewModel;
                }).ToList();
            IsSelectedChanged(null, null);
        }

        private void IsSelectedChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (Directories.All(vm => vm.IsSelected))
            {
                AreAllSelected = true;
            }
            else if (!Directories.Any(vm => vm.IsSelected))
            {
                AreAllSelected = false;
            }
            else
            {
                AreAllSelected = null;
            }
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
