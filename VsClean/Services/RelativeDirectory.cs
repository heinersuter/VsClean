namespace VsClean.Services
{
    public struct RelativeDirectory
    {
        public string RootDirectory { get; }

        public string AbsolutePath { get; }

        public string RelativePath { get; }

        public RelativeDirectory(string rootDirectory, string absolutePath)
        {
            RootDirectory = rootDirectory;
            AbsolutePath = absolutePath;
            RelativePath = AbsolutePath.Substring(RootDirectory.Length + 1);
        }
    }
}
