using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VsClean.V2.CommandLine
{
    public class ArgsParserItems<T> : IEnumerable<T> where T : IArgsParserItem
    {
        private readonly List<T> _items = new List<T>();

        public T this[string index]
        {
            get { return _items.SingleOrDefault(flag => flag.Name == index); }
            set { _items.Add(value); }
        }

        public bool Contains(string name)
        {
            return _items.Any(arg => arg.Name == name);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}