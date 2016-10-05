namespace VsClean.Cli
{
    public class CliArguments
    {
        public CliArguments(string rootDirectory, bool interactive, bool verbose)
        {
            RootDirectory = rootDirectory;
            Interactive = interactive;
            Verbose = verbose;
        }

        public string RootDirectory { get; set; }

        public bool Interactive { get; }

        public bool Verbose { get; }
    }
}