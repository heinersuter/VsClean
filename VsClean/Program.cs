using System;
using System.IO;
using System.Linq;

namespace VsClean
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            ////args = new[] { @"C:\Projects\Zurich\MyZ\Trunk\Implementation\Main\Source" };
            if (args.Length > 1)
            {
                Console.WriteLine("Only one argument can be provided.");
                PrintUsage();
                return -1;
            }

            string rootDirectory;
            if (args.Length == 1)
            {
                if (!Directory.Exists(args[0]))
                {
                    Console.WriteLine("The provided root directory does not exist.");
                    PrintUsage();
                    return -1;
                }
                rootDirectory = args[0];
            }
            else
            {
                rootDirectory = Environment.CurrentDirectory;
            }
            Console.WriteLine($"Looking in: {rootDirectory}");

            var directoryCleaner = new DirectoryCleaner(rootDirectory);
            var directories = directoryCleaner.FindDirectoriesToDelete().ToList();

            if (!directories.Any())
            {
                Console.WriteLine("No directories found.");
                return 0;
            }

            Console.WriteLine();
            Console.WriteLine($"These {directories.Count} directories were found:");
            foreach (var directory in directories)
            {
                Console.WriteLine(directory);
            }

            Console.WriteLine();
            Console.WriteLine($"Do you want to delete all {directories.Count} of them? (y/n)");
            var answer = Console.ReadLine();
            if (answer != null && answer.ToLowerInvariant().StartsWith("y"))
            {
                directoryCleaner.Delete(directories);
            }

            return 0;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: vsclean [rootDirectory]");
        }
    }
}
