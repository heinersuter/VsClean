using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VsClean
{
    public class DirectoryCleaner
    {
        private static readonly string[] DeleteDirectoryList = { "bin", "obj", "TestResults" };
        private static readonly string[] ExcludeList = { @"\packages\", @"\node_modules\" };

        private readonly string _rootDirectory;

        public DirectoryCleaner(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }

        public IEnumerable<string> FindDirectoriesToDelete()
        {
            var allDirectories = new List<string>();
            allDirectories.AddRange(DeleteDirectoryList.SelectMany(dir => Directory.GetDirectories(_rootDirectory, dir, SearchOption.AllDirectories)));

            RemoveIfParentInList(allDirectories);

            var directoriesToDelete = allDirectories.Select(GetRelativePath);
            directoriesToDelete = directoriesToDelete.Where(DoesNotContainAnExcludedPart);

            return directoriesToDelete.OrderBy(d => d);
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

        private string GetRelativePath(string directoryPath)
        {
            return directoryPath.Substring(_rootDirectory.Length);
        }

        private static bool DoesNotContainAnExcludedPart(string d)
        {
            return !ExcludeList.Any(d.Contains);
        }

        public void Delete(IEnumerable<string> directoryPaths)
        {
            foreach (var directoryPath in directoryPaths)
            {
                var path = Path.Combine(_rootDirectory, directoryPath.Substring(1));
                try
                {
                    Directory.Delete(path, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"!Error: {directoryPath} not deleted: {ex.Message}");
                }
            }
        }
    }
}
