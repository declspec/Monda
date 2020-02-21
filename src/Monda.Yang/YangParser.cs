using System;
using System.Collections.Generic;

namespace Monda.Yang {
    public static partial class YangParser {
        public static IReadOnlyList<YangStatement> Parse(string yang) {
            var result = StatementParser.Many(1).Parse(yang.AsSpan());

            if (!result.Success)
                throw new FormatException("string did not contain a valid YANG document");

            return result.Value;
        }
    }
}
