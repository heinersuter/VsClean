namespace VsClean.V2.CommandLine
{
    public class ArgsParserItem<T> : IArgsParserItem
    {
        public ArgsParserItem(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }

        public string Description { get; }

        public T Value { get; set; }
    }
}