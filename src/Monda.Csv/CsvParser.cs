using System;
using System.Collections.Generic;

namespace Monda.Csv {
    public static partial class CsvParser {
        public static IReadOnlyList<IReadOnlyList<string>> Parse(string csv) {
            var result = RowParser.ManyUntil(EofParser).Parse(csv.AsSpan());

            if (!result.Success)
                throw new FormatException("invalid csv content");

            return result.Value.Item1;
        }
    }
}
