using NUnit.Framework;
using Shouldly;
using VsClean.CommandLine;

namespace VsClean.Tests.CommandLine
{
    public class ArgsParserTests
    {
        [Test]
        public void ExpectFlag_ValidFlagWithDescription_FlagIsAddedToList()
        {
            var argsParser = new ArgsParser();

            argsParser.ExpectFlag("x", "Description for x");

            argsParser.Flags["x"].Description.ShouldBe("Description for x");
        }

        [Test]
        public void ExpectOption_ValidoptionWithDescription_OptionWithDescriptionIsAddedToList()
        {
            var argsParser = new ArgsParser();

            argsParser.ExpectOption("x", "placeholderX", "Description for x", true);

            argsParser.Options["x"].Description.ShouldBe("Description for x");
            argsParser.Options["x"].Placeholder.ShouldBe("placeholderX");
            argsParser.Options["x"].IsMandatory.ShouldBe(true);
        }

        [Test]
        public void Parse_ExpectedFlagIsProvided_FlagIsTrue()
        {
            var argsParser = new ArgsParser();
            argsParser.ExpectFlag("x", "Description for x");

            var parseResult = argsParser.Parse(new[] { "-x" });

            parseResult.ShouldBe(true);
            bool flagValue = argsParser.Flags["x"];
            flagValue.ShouldBe(true);
        }

        [Test]
        public void Parse_ExpectedOptionIsProvided_ValueIsSet()
        {
            var argsParser = new ArgsParser();
            argsParser.ExpectOption("x", "placeholderX", "Description for x", true);

            var parseResult = argsParser.Parse(new[] { "-x", "ValueOfX" });

            parseResult.ShouldBe(true);
            string optionValue = argsParser.Options["x"];
            optionValue.ShouldBe("ValueOfX");
        }

        [Test]
        public void Parse_UnexpectedFlag_ErrorIsIndicated()
        {
            var argsParser = new ArgsParser();

            var parseResult = argsParser.Parse(new[] { "-x" });

            parseResult.ShouldBe(false);
            argsParser.GetErrorText().Contains("-x").ShouldBe(true);
        }

        [Test]
        public void Parse_UnexpectedOption_ErrorIsIndicated()
        {
            var argsParser = new ArgsParser();

            var parseResult = argsParser.Parse(new[] { "-x", "ValueOfX" });

            parseResult.ShouldBe(false);
            argsParser.GetErrorText().Contains("-x").ShouldBe(true);
        }

        [Test]
        public void Parse_OptionWithNoValue_ErrorIsIndicated()
        {
            var argsParser = new ArgsParser();
            argsParser.ExpectOption("x", "placeholderX", "Description for x", true);

            var parseResult = argsParser.Parse(new[] { "-x" });

            parseResult.ShouldBe(false);
            argsParser.GetErrorText().Contains("-x").ShouldBe(true);
            argsParser.GetErrorText().Contains("placeholderX").ShouldBe(true);
        }

        [Test]
        public void GetUsage_ValidFlag_FlagIsIncludedInUsage()
        {
            var argsParser = new ArgsParser();
            argsParser.ExpectFlag("x", "Description for x");

            var usage = argsParser.GetUsage();

            usage.Contains("[-x]").ShouldBe(true);
            usage.Contains("Description for x").ShouldBe(true);
        }

        [Test]
        public void GetUsage_ValidOption_FlagIsIncludedInUsage()
        {
            var argsParser = new ArgsParser();
            argsParser.ExpectOption("x", "placeholderX", "Description for x", true);

            var usage = argsParser.GetUsage();

            usage.Contains("-x ").ShouldBe(true);
            usage.Contains("Description for x").ShouldBe(true);
            usage.Contains("<placeholderX>").ShouldBe(true);
            usage.Contains("mandatory").ShouldBe(true);
        }
    }
}