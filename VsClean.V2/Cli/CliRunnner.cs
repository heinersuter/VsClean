using System;
using System.IO;
using System.Linq;
using VsClean.V2.Services;

namespace VsClean.V2.Cli
{
    public class CliRunnner
    {
        public static void Start(CliArguments arguments)
        {
            if (arguments.RootDirectory == null)
            {
                arguments.RootDirectory = Environment.CurrentDirectory;
            }

            if (!Directory.Exists(arguments.RootDirectory))
            {
                Console.WriteLine();
                Console.WriteLine($"The directory does not exist: {arguments.RootDirectory}");
                return;
            }

            var directoryService = new DirectoryService(arguments.RootDirectory);

            FindDirectories(directoryService, arguments.Verbose);

            if (arguments.Interactive)
            {
                Console.WriteLine();
                Console.WriteLine($"Do you want to delete all {directoryService.FoundDirectories.Count} of them? (y/n)");
                var answer = Console.ReadLine();
                if (answer != null && answer.ToLowerInvariant().StartsWith("y"))
                {
                    DeleteDirectories(directoryService, arguments.Verbose);
                }
            }
            else
            {
                DeleteDirectories(directoryService, arguments.Verbose);
            }
        }

        private static void FindDirectories(DirectoryService directoryService, bool verbose)
        {
            directoryService.FindDirectoriesToDelete();

            if (verbose)
            {
                Console.WriteLine();
                Console.WriteLine("Ignored directories:");
                foreach (var directory in directoryService.IgnoredDirectories)
                {
                    Console.WriteLine($" - {directory.RelativePath}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Found directories:");
            foreach (var directory in directoryService.FoundDirectories)
            {
                Console.WriteLine($" + {directory.RelativePath}");
            }
        }

        private static void DeleteDirectories(DirectoryService directoryService, bool verbose)
        {
            directoryService.Delete();

            if (directoryService.UndeletedDirectories.Any())
            {
                Console.WriteLine();
                Console.WriteLine("Not deleted:");
                foreach (var directory in directoryService.UndeletedDirectories)
                {
                    Console.WriteLine($" ! {directory.Key.RelativePath}");
                    if (verbose)
                    {
                        Console.WriteLine($"   -> {directory.Value}");
                    }
                }
            }
        }
    }
}