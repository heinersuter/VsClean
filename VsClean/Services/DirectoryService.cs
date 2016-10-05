using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VsClean.Services
{
    public class DirectoryService
    {
        private readonly string _rootDirectory;
        private static readonly string[] ExcludeList = { @"\packages\", @"\node_modules\" };

        public DirectoryService(string rootDirectory)
        {
            _rootDirectory = rootDirectory.EndsWith(@"\") ? rootDirectory.TrimEnd('\\') : rootDirectory;
        }

        public IList<RelativeDirectory> FoundDirectories { get; private set; }

        public IList<RelativeDirectory> IgnoredDirectories { get; private set; }

        public IDictionary<RelativeDirectory, string> UndeletedDirectories { get; private set; }

        public void FindDirectoriesToDelete()
        {
            var allDirectories = new List<string>();
            allDirectories.AddRange(Directory.GetDirectories(_rootDirectory, "bin", SearchOption.AllDirectories));
            allDirectories.AddRange(Directory.GetDirectories(_rootDirectory, "obj", SearchOption.AllDirectories));
            RemoveIfParentInList(allDirectories);

            var relativeDirecotories = allDirectories.Select(absolutePath => new RelativeDirectory(_rootDirectory, absolutePath)).OrderBy(direcotry => direcotry.RelativePath);

            IgnoredDirectories = relativeDirecotories.Where(DoesContainAnExcludedPart).ToList();

            FoundDirectories = relativeDirecotories.Except(IgnoredDirectories).ToList();
        }

        public void Delete()
        {
            var undeleted = new Dictionary<RelativeDirectory, string>();
            foreach (var directory in FoundDirectories)
            {
                try
                {
                    Directory.Delete(directory.AbsolutePath, true);
                }
                catch (Exception ex)
                {
                    undeleted.Add(directory, ex.Message);
                }
            }
            UndeletedDirectories = undeleted;
        }

        private static void RemoveIfParentInList(List<string> allDirectories)
        {
            for (var i = allDirectories.Count - 1; i >= 0; i--)
            {
                var current = allDirectories[i];
                if (allDirectories.Except(new[] { current }).Any(d => current.StartsWith(d)))
                {
                    allDirectories.RemoveAt(i);
                }
            }
        }

        private static bool DoesContainAnExcludedPart(RelativeDirectory directory)
        {
            return ExcludeList.Any((@"\" + directory.RelativePath).Contains);
        }
    }
}
