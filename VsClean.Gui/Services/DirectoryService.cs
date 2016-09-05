using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VsClean.Gui.Services
{
    public class DirectoryService
    {
        private static readonly string[] _excludeList = { @"\packages\", @"\node_modules\" };

        public IEnumerable<RelativeDirecotry> FindDirectoriesToDelete(string rootDirectory)
        {
            rootDirectory = rootDirectory.EndsWith(@"\") ? rootDirectory.TrimEnd('\\') : rootDirectory;

            var allDirectories = new List<string>();
            allDirectories.AddRange(Directory.GetDirectories(rootDirectory, "bin", SearchOption.AllDirectories));
            allDirectories.AddRange(Directory.GetDirectories(rootDirectory, "obj", SearchOption.AllDirectories));
            RemoveIfParentInList(allDirectories);

            var directoriesToDelete = allDirectories.Select(absolutePath => new RelativeDirecotry(rootDirectory, absolutePath));
            directoriesToDelete = directoriesToDelete.Where(DoesNotContainAnExcludedPart);

            return directoriesToDelete.OrderBy(direcotry => direcotry.RelativePath);
        }

        public void Delete(IEnumerable<RelativeDirecotry> directories)
        {
            foreach (var absolutePath in directories.Select(directory => directory.AbsolutePath))
            {
                try
                {
                    Directory.Delete(absolutePath, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"!Error: {absolutePath} not deleted: {ex.Message}");
                }
            }
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

        private static bool DoesNotContainAnExcludedPart(RelativeDirecotry directory)
        {
            return !_excludeList.Any((@"\" + directory.RelativePath).Contains);
        }
    }
}
