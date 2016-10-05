using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace VsClean.CommandLine
{
    public class ArgsParser
    {
        private const string ParserItemPattern = @"^(-|--|/)(\w+)$";

        private readonly StringBuilder _errorText = new StringBuilder();

        public ArgsParserItems<ArgsParserFlag> Flags { get; } = new ArgsParserItems<ArgsParserFlag>();

        public ArgsParserItems<ArgsParserOption> Options { get; } = new ArgsParserItems<ArgsParserOption>();

        public void ExpectFlag(string name, string description)
        {
            Flags[name] = new ArgsParserFlag(name, description);
        }

        public void ExpectOption(string name, string placeholder, string description, bool isMandatory)
        {
            Options[name] = new ArgsParserOption(name, placeholder, description, isMandatory);
        }

        public bool Parse(string[] args)
        {
            _errorText.Clear();

            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var match = Regex.Match(arg, ParserItemPattern);
                if (!match.Success)
                {
                    continue;
                }
                var name = match.Groups[2].Value;

                if (Options.Contains(name))
                {
                    if (i + 1 < args.Length && !Regex.IsMatch(args[i + 1], ParserItemPattern))
                    {
                        Options[name].Value = args[i + 1];
                    }
                    else
                    {
                        _errorText.AppendLine($"The option {arg} must be followed by a value <{Options[name].Placeholder}>.");
                        return false;
                    }
                }
                else if (Flags.Contains(name))
                {
                    Flags[name].Value = true;
                }
                else
                {
                    _errorText.AppendLine($"The option {arg} is unknown.");
                }
            }

            return _errorText.Length == 0;
        }

        public string GetErrorText()
        {
            var errorText = _errorText.ToString();
            if (errorText.EndsWith(Environment.NewLine))
            {
                return errorText.Substring(0, errorText.Length - Environment.NewLine.Length);
            }
            return errorText;
        }

        public string GetUsage()
        {
            var sb = new StringBuilder();

            sb.Append("Usage: ");
            sb.Append(Assembly.GetExecutingAssembly().GetName().Name);
            foreach (var flag in Flags)
            {
                sb.Append($" [-{flag.Name}]");
            }
            foreach (var option in Options)
            {
                sb.Append(" ");
                if (!option.IsMandatory)
                {
                    sb.Append("[");
                }
                sb.Append($"-{option.Name} <{option.Placeholder}>");
                if (!option.IsMandatory)
                {
                    sb.Append("]");
                }
            }
            sb.AppendLine();
            sb.AppendLine();

            sb.AppendLine("Options:");
            foreach (var flag in Flags)
            {
                sb.Append($"  -{flag.Name}    {flag.Description}");
                sb.AppendLine();
            }
            foreach (var option in Options)
            {
                sb.Append($"  -{option.Name}    {option.Description}");
                if (option.IsMandatory)
                {
                    sb.Append(" (mandatory)");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}