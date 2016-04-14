using Alsolos.Commons.Wpf.Mvvm;

using Ninject;

namespace VsClean.Gui.Views
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel(IKernel kernel)
        {
            MainViewModel = kernel.Get<MainViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get { return BackingFields.GetValue<MainViewModel>(); }
            set { BackingFields.SetValue(value); }
        }
    }
}
