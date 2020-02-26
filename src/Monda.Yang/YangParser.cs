using System;
using System.Collections.Generic;
using Monda.Yang.Models;

namespace Monda.Yang {
    public static partial class YangParser {
        public static IReadOnlyList<YangStatement> ParseStatements(string yang) {
            return ParseStatements(yang.AsSpan());
        }

        public static IReadOnlyList<YangStatement> ParseStatements(ReadOnlySpan<char> span) {
            var trace = new ParserTrace();
            var result = StatementParser.Parse(span, trace);

            if (!result.Success)
                throw new FormatException($"error parsing YANG statements at position: {trace.Position}");

            return result.Value;
        }

        public static IYangModule ParseModule(ReadOnlySpan<char> span) {
            var statements = ParseStatements(span);

            if (statements.Count != 1)
                throw new ArgumentException("span must contain exactly one statement");

            return ParseModule(statements[0]);
        }

        public static IYangModule ParseModule(YangStatement statement) {
            if (statement == null)
                throw new ArgumentNullException(nameof(statement));

            if (!string.Equals(statement.Keyword, "module", StringComparison.OrdinalIgnoreCase) && !string.Equals(statement.Keyword, "submodule", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentOutOfRangeException(nameof(statement), "statement must be either a module or submodule");

            return YangStatementParsers.ParseModule(statement);
        }
    }
}
