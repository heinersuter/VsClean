using System;
using VsClean.Cli;
using VsClean.CommandLine;
using VsClean.Gui;

namespace VsClean
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //args = new[] { "-x" };

            var argsParser = new ArgsParser();
            argsParser.ExpectFlag("v", "Print verbose output.");
            argsParser.ExpectFlag("g", "Start in GUI mode.");
            argsParser.ExpectFlag("y", "No interaction, all directories are deleted.");
            argsParser.ExpectFlag("h", "Print this help message.");
            argsParser.ExpectOption("d", "rootDircetory", "The directory to clean", false);

            var isValid = argsParser.Parse(args);
            if (!isValid)
            {
                Console.WriteLine(argsParser.GetErrorText());
                Console.WriteLine();
                Console.WriteLine(argsParser.GetUsage());
                return;
            }

            if (argsParser.Flags["h"])
            {
                Console.WriteLine(argsParser.GetUsage());
            }
            else if (argsParser.Flags["g"])
            {
                GuiStarter.Start(new GuiArguments(argsParser.Options["d"], argsParser.Flags["v"]));
            }
            else
            {
                CliRunnner.Start(new CliArguments(argsParser.Options["d"], !argsParser.Flags["y"], argsParser.Flags["v"]));
            }
        }
    }
}