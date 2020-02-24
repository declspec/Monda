using System.Collections.Generic;

namespace Monda {
    public class ParserTrace {
        public int Position { get; internal set; }
        public IList<string> Parsers { get; }

        public ParserTrace() {
            Position = 0;
            Parsers = new List<string>();
        }
    }
}
