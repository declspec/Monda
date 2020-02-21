using System;
using System.Collections.Generic;
using System.Text;

namespace Monda.Yang {
    public static partial class YangParser {
        private static readonly Parser<char, Range> LineCommentParser = Parser.Match<char>(ch => ch != '\n')
            .PrecededBy(Parser.Literal("//".AsMemory()));

        private static readonly Parser<char, Range> BlockCommentParser = Parser.MatchUntil(ch => true, Parser.Literal("*/".AsMemory()))
            .Map((res, data) => res.Value.Item1)
            .PrecededBy(Parser.Literal("/*".AsMemory()));

        private static readonly Parser<char, Range> CommentParser = LineCommentParser.Or(BlockCommentParser);

        public static readonly Parser<char, int> SeparatorParser = Parser.Match<char>(char.IsWhiteSpace, 1)
            .Or(CommentParser)
            .SkipMany(1);

        private static readonly Parser<char, int> OptionalSeparatorParser = SeparatorParser.Optional();

        private static readonly Parser<char, string> IdentifierParser = new Parser<char, string>((data, start) => {
            var pos = start;

            while (pos < data.Length) {
                var ch = data[pos];

                // char must be an underscore or letter when it is the first char
                // otherwise, it may also be a full-stop, hyphen or digit.
                if (ch != '_' && !char.IsLetter(ch) && (pos == start || (ch != '.' && ch != '-' && !char.IsDigit(ch))))
                    break;

                ++pos;
            }

            var id = data.Slice(start, pos - start);

            return id.IsEmpty || id.StartsWith("xml".AsSpan(), StringComparison.OrdinalIgnoreCase)
                ? ParseResult.Fail<string>()
                : ParseResult.Success(id.ToString(), start, pos - start);
        });

        public static readonly Parser<char, Tuple<string, string>> KeywordParser = IdentifierParser
            .FollowedBy(Parser.Is(':'))
            .Optional()
            .Then(IdentifierParser);

        private static readonly Parser<char, string> SingleQuotedStringParser = Parser.Match<char>(ch => ch != '\'')
            .Between(Parser.Is('\''))
            .Map(CopyToString);

        private static readonly Parser<char, Range> EscapeSequencesParser = new Parser<char, Range>((data, start) => {
            var pos = start;

            while ((pos + 1) < data.Length && data[pos] == '\\')
                ++pos;

            return pos == start ? ParseResult.Fail(Range.Failure) : ParseResult.Success(new Range(start, pos - start), start, pos - start);
        });

        private static readonly Parser<char, string> DoubleQuotedStringParser = Parser.Match<char>(ch => ch != '\\' && ch != '"', 1)
            .Then(EscapeSequencesParser.Optional(Range.Failure))
            .Many()
            .Optional()
            .Between(Parser.Is('"'))
            .Map(CopyToEscapedString);

        private static readonly Parser<char, string> UnquotedAgumentParser = Parser.MatchUntil(ch => "'\";{}".IndexOf(ch) < 0, SeparatorParser)
            .Map((res, data) => data.Slice(res.Value.Item1.Start, res.Value.Item1.Length).ToString());

        private static readonly Parser<char, char> ConcatParser = Parser.Is('+').Between(OptionalSeparatorParser);

        private static readonly Parser<char, string> QuotedArgumentParser = DoubleQuotedStringParser.Or(SingleQuotedStringParser)
            .Then(DoubleQuotedStringParser.Or(SingleQuotedStringParser)
                .PrecededBy(ConcatParser)
                .Many()
                .Optional())
            .Map((res, data) => {
                if (!(res.Value.Item2?.Count > 0))
                    return res.Value.Item1;

                var len = res.Value.Item1.Length;

                foreach (var str in res.Value.Item2)
                    len += str.Length;

                return StringUtlities.CreateString(len, res.Value, (span, state) => {
                    var offset = state.Item1.Length;
                    state.Item1.AsSpan().CopyTo(span);

                    foreach (var str in state.Item2) {
                        span = span.Slice(offset);
                        str.AsSpan().CopyTo(span);
                        offset += str.Length;
                    }
                });
            });

        private static readonly Parser<char, string> ArgumentParser = UnquotedAgumentParser
            .Or(QuotedArgumentParser);

        private static readonly Parser<char, YangStatement> StatementParser = KeywordParser
            .Then(ArgumentParser.PrecededBy(SeparatorParser))
            .Then(Parser.Is(';').Map((res, data) => default(IReadOnlyList<YangStatement>))
                .Or(SeparatorParser
                    .SkipMany()
                    .Map((res, data) => default(IReadOnlyList<YangStatement>))
                    .Between(Parser.Is('{'), Parser.Is('}'))))
                .PrecededBy(OptionalSeparatorParser)
            .Map((res, data) => new YangStatement(res.Value.Item1.Item1.Item1, res.Value.Item1.Item1.Item2, res.Value.Item1.Item2, res.Value.Item2));

        private static readonly Parser<char, IReadOnlyList<YangStatement>> MultipleStatementParser = StatementParser
            .Many(1);


        //
        // Mapping helpers
        //

        private static string CopyToString(ParseResult<Range> res, ReadOnlySpan<char> data) {
            return data.Slice(res.Value.Start, res.Value.Length).ToString();
        }

        private static string CopyToEscapedString(ParseResult<IReadOnlyList<Tuple<Range, Range>>> result, ReadOnlySpan<char> data) {
            // This map function is very complicated
            // It should handle all of the YANG escaping/indenting in double quoted string rules from https://tools.ietf.org/html/rfc7950
            // It should also perform as few allocations as possible; it is a relatively hot path in the parser

            // Easy option 1) Empty string
            if (result.Length == 0)
                return string.Empty;

            // Easy option 2) No escape characters and no newlines
            if (result.Value[0].Item1.Length == result.Length) {
                var slice = data.Slice(result.Value[0].Item1.Start, result.Value[0].Item1.Length);

                if (slice.IndexOf('\n') < 0)
                    return slice.ToString();
            }

            // Compute how indented the start of the string is, which will be used to offset the indentation of following lines
            // i.e
            //      "string starts here
            //          indents here"
            // Should have a 4-space indent, not 8. Tabs are counted as 8 spaces as per the spec.
            var pos = result.Start;
            var origin = data.Slice(0, pos).LastIndexOf('\n') + 1; // Find the previous newline and start from there
            var tabcount = CountOccurences(data.Slice(origin, pos - origin), '\t');
            var indent = (pos - origin - tabcount) + (tabcount * 8); // Number of characters minus number of tabs, plus number of tabs * tabsize (8).

            // Build the escaped string
            // TODO: Maybe some analysis on how often the final StringBuilder.Length is < result.Length
            var sb = new StringBuilder(result.Length);

            foreach (var value in result.Value) {
                // Handle the first item in the Tuple: the string without escape characters
                //  it may need indentation handling
                AppendIndentedString(sb, indent, data.Slice(value.Item1.Start, value.Item1.Length));

                // Handle the escaped string
                if (value.Item2.Length > 0)
                    AppendEscapedString(sb, data.Slice(value.Item2.Start, value.Item2.Length));
            }

            return sb.ToString();
        }

        private static int CountOccurences(in ReadOnlySpan<char> chars, char ch) {
            var count = 0;

            for (var i = 0; i < chars.Length; ++i) {
                if (chars[i] == ch)
                    ++count;
            }

            return count;
        }

        private static StringBuilder AppendIndentedString(StringBuilder sb, int indent, ReadOnlySpan<char> str) {
            var newline = str.IndexOf('\n');

            while (newline >= 0) {
                Append(sb, str.Slice(0, ++newline));

                // Try and offset by the indent, but stop if a newline / non whitespace
                // character is encountered so we don't truncate data
                int skip = 0;

                for (; newline < str.Length && skip < indent; ++newline) {
                    if (str[newline] == '\n' || !char.IsWhiteSpace(str[newline]))
                        break;
                    skip += str[newline] == '\t' ? 8 : 1;
                }

                str = str.Slice(newline);
                newline = str.IndexOf('\n');
            }

            return Append(sb, str);
        }

        private static StringBuilder AppendEscapedString(StringBuilder sb, ReadOnlySpan<char> str) {
            // Escape strings are always in the following format (see parser)
            // \x\y\z\a\b\c
            for (var i = 0; i < str.Length; i += 2) {
                switch (str[i + 1]) {
                    // Currently only 4 recognised escape sequences according to RFC7950
                    case '"':
                    case '\\':
                        sb.Append(str[i + 1]);
                        break;
                    case 'n':
                        sb.Append('\n');
                        break;
                    case 't':
                        sb.Append('\t');
                        break;
                    default:
                        // Unknown escape sequence, just append it
                        Append(sb, str.Slice(i, 2));
                        break;
                }
            }

            return sb;
        }

        private static StringBuilder Append(StringBuilder sb, in ReadOnlySpan<char> data) {
            if (data.IsEmpty)
                return sb;

            unsafe {
                fixed (char* ptr = &data[0]) 
                    return sb.Append(ptr, data.Length);
            }
        }
    }
}
