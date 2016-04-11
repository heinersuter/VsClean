﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VsClean
{
    public class DirectoryCleaner
    {
        private static readonly string[] _excludeList = { @"\packages\", @"\node_modules\" };

        private readonly string _rootDirectory;

        public DirectoryCleaner(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }

        public IEnumerable<string> FindDirectoriesToDelete()
        {
            var allDirectories = new List<string>();
            allDirectories.AddRange(Directory.GetDirectories(_rootDirectory, "bin", SearchOption.AllDirectories));
            allDirectories.AddRange(Directory.GetDirectories(_rootDirectory, "obj", SearchOption.AllDirectories));
            allDirectories.Sort();
            for (var i = allDirectories.Count - 1; i >= 0; i--)
            {
                var current = allDirectories[i];
                var last = i > 0 ? allDirectories[i - 1] : null;
                if (last != null && current.StartsWith(last))
                {
                    allDirectories.RemoveAt(i);
                }
            }

            var directoriesToDelete = allDirectories.Select(GetRelativePath);
            directoriesToDelete = directoriesToDelete.Where(DoesNotContainAnExcludedPart);

            return directoriesToDelete;
        }

        private string GetRelativePath(string directoryPath)
        {
            return directoryPath.Substring(_rootDirectory.Length);
        }

        private static bool DoesNotContainAnExcludedPart(string d)
        {
            return !_excludeList.Any(d.Contains);
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
