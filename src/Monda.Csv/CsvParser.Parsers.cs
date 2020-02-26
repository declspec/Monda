using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monda.Csv {
    public static partial class CsvParser {
        private static Parser<char, char> EofParser = new Parser<char, char>((data, start, trace) => {
            return start == data.Length ? ParseResult.Success('\0', start, 0) : ParseResult.Fail<char>();
        });

        private static Parser<char, Range> EscapedDoubleQuotes = new Parser<char, Range>((data, start, trace) => {
            var pos = start;

            while ((pos + 1) < data.Length && (data[pos] == '"' && data[pos + 1] == '"')) {
                pos += 2;
            }

            return pos == start
                ? ParseResult.Fail(Range.Failure)
                : ParseResult.Success(new Range(start, pos - start), start, pos - start);
        });

        private static Parser<char, string> UnquotedStringParser = Parser.TakeWhile<char>(ch => ch != '\n' && ch != ',')
            .Map((res, data) => res.Length == 0 ? string.Empty : data.Slice(res.Start, res.Length).ToString());

        private static Parser<char, string> QuotedStringParser = Parser.TakeWhile<char>(ch => ch != '"', 1)
            .Then(EscapedDoubleQuotes.Optional(Range.Failure))
            .Many()
            .Map((res, data) => {
                // Fast cases, no double quotes or only double quotes
                if (res.Length == 0)
                    return string.Empty;

                if (res.Value.Count == 1) {
                    var val = res.Value[0];

                    if (val.Item1.Length == res.Length)
                        return data.Slice(val.Item1.Start, val.Item1.Length).ToString();

                    if (val.Item2.Length == res.Length)
                        return new string('"', val.Item2.Length / 2);
                }

                // Slow case, need to build the resulting string
                var sb = new StringBuilder(res.Value.Sum(tpl => tpl.Item1.Length + tpl.Item2.Length));

                foreach (var value in res.Value) {
                    if (value.Item1.Length > 0) {
                        // TODO: In netstandard2.1 the .Append(ReadOnlySpan<char>) overload has been added
                        //  which would save us needing any unsafe code here, but legacy .NET Framework
                        //   ("legacy" being anything pre .NET 5) will not support netstandard2.1
                        unsafe {
                            fixed (char* ptr = &data[value.Item1.Start])
                                sb.Append(ptr, value.Item1.Length);
                        }
                    }

                    if (value.Item2.Length > 0)
                        sb.Append('"', value.Item2.Length / 2);
                }

                return sb.ToString();
            })
            .Optional(string.Empty)
            .Between(Parser.Is('"'), Parser.Is('"'));

        private static Parser<char, string> StringParser = QuotedStringParser.Then(UnquotedStringParser)
            .Map((res, data) => (res.Length == 0 ? string.Empty : (string.IsNullOrEmpty(res.Value.Item2) ? res.Value.Item1 : res.Value.Item1 + res.Value.Item2)))
            .Or(UnquotedStringParser);

        private static Parser<char, IReadOnlyList<string>> RowParser = StringParser.FollowedBy(Parser.Is(',').Optional())
            .ManyUntil(Parser.Is('\n').Or(EofParser))
            .Map((res, data) => res.Value.Item1);
    }
}
