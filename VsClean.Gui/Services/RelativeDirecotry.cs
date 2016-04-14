namespace VsClean.Gui.Services
{
    public struct RelativeDirecotry
    {
        public string RootDirectory { get; }

        public string AbsolutePath { get; }

        public string RelativePath { get; }

        public RelativeDirecotry(string rootDirectory, string absolutePath)
        {
            RootDirectory = rootDirectory;
            AbsolutePath = absolutePath;
            RelativePath = AbsolutePath.Substring(RootDirectory.Length + 1);
        }
    }
}
