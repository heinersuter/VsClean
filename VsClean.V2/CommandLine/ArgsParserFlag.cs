namespace VsClean.V2.CommandLine
{
    public class ArgsParserFlag : ArgsParserItem<bool>
    {
        public ArgsParserFlag(string name, string description)
            : base(name, description)
        {
        }

        public static implicit operator bool(ArgsParserFlag flag)
        {
            return flag.Value;
        }
    }
}