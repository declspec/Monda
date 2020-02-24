using System;
using System.Collections.Generic;

namespace Monda.Yang {
    public static partial class YangParser {
        public static IReadOnlyList<YangStatement> Parse(string yang) {
            var trace = new ParserTrace();
            var result = StatementParser.Parse(yang.AsSpan(), trace);

            if (!result.Success)
                throw new FormatException("string did not contain a valid YANG document");

            return result.Value;
        }
    }
}
