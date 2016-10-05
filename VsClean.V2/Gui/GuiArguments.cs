namespace VsClean.V2.Gui
{
    public class GuiArguments
    {
        public GuiArguments(string rootDirectory, bool verbose)
        {
            RootDirectory = rootDirectory;
            Verbose = verbose;
        }

        public string RootDirectory { get; }

        public bool Verbose { get; }
    }
}