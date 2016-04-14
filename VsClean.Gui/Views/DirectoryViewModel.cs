using Alsolos.Commons.Wpf.Mvvm;

using VsClean.Gui.Services;

namespace VsClean.Gui.Views
{
    public class DirectoryViewModel : ViewModel
    {
        public DirectoryViewModel(RelativeDirecotry directory)
        {
            Directory = directory;
            IsSelected = true;
        }

        public RelativeDirecotry Directory { get; }

        public bool IsSelected
        {
            get { return BackingFields.GetValue<bool>(); }
            set { BackingFields.SetValue(value); }
        }
    }
}
