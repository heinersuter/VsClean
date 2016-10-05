namespace VsClean.CommandLine
{
    public class ArgsParserOption : ArgsParserItem<string>
    {
        public ArgsParserOption(string name, string placeholder, string description, bool isMandatory)
            : base(name, description)
        {
            Placeholder = placeholder;
            IsMandatory = isMandatory;
        }

        public string Placeholder { get; }

        public bool IsMandatory { get; }

        public static implicit operator string(ArgsParserOption option)
        {
            return option.Value;
        }
    }
}