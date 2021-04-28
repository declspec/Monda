using System;
using System.Collections.Generic;
using System.Text;

namespace Monda.Yang {
    public static partial class YangParser {
        private static readonly Parser<char, char> EndStatementParser = Parser.Is(';')
            .WithName(nameof(EndStatementParser));

        private static readonly Parser<char, Range> LineCommentParser = Parser.TakeWhile<char>(ch => ch != '\n')
            .PrecededBy(Parser.IsSequence("//".ToCharArray()))
            .WithName(nameof(LineCommentParser));

        private static readonly Parser<char, Range> BlockCommentParser = Parser.TakeUntil(Parser.IsSequence("*/".ToCharArray()))
            .PrecededBy(Parser.IsSequence("/*".ToCharArray()))
            .Map((res, data) => res.Value.Item1)
            .WithName(nameof(BlockCommentParser));

        private static readonly Parser<char, Range> CommentParser = LineCommentParser.Or(BlockCommentParser)
            .WithName(nameof(CommentParser));

        private static readonly Parser<char, int> SeparatorParser = Parser.TakeWhile<char>(char.IsWhiteSpace, min: 1)
            .Or(CommentParser)
            .SkipMany(min: 1)
            .WithName(nameof(SeparatorParser));

        private static readonly Parser<char, int> OptionalSeparatorParser = SeparatorParser.Optional()
            .WithName(nameof(OptionalSeparatorParser));

        // Identifier must start with an underscore or letter and then be followed by letters, digits, periods or hyphens
        private static ParserPredicate<char> IdentifierPredicate = (data, index) => data[index] == '_' 
            || char.IsLetter(data[index]) 
            || (index > 0 && (data[index] == '.' || data[index] == '-' || char.IsDigit(data[index]))); // Can be . or - or digit iif not the first character.

        private static readonly Parser<char, string> IdentifierParser = Parser.TakeWhile(IdentifierPredicate, min: 1)
            .Map(CopyToString)
            .WithName(nameof(IdentifierParser));

        private static readonly Parser<char, Tuple<string, string>> KeywordParser = IdentifierParser
            .FollowedBy(Parser.Is(':'))
            .Optional()
            .Then(IdentifierParser)
            .WithName(nameof(KeywordParser));

        private static readonly Parser<char, string> SingleQuotedStringParser = Parser.TakeWhile<char>(ch => ch != '\'')
            .Between(Parser.Is('\''))
            .Map(CopyToString)
            .WithName(nameof(SingleQuotedStringParser));

        private static readonly Parser<char, Range> EscapeSequencesParser = new Parser<char, Range>(nameof(EscapeSequencesParser), (data, start, trace) => {
            var pos = start;

            while ((pos + 1) < data.Length && data[pos] == '\\')
                pos += 2;

            return pos == start ? ParseResult.Fail(Range.Failure) : ParseResult.Success(new Range(start, pos - start), start, pos - start);
        });

        private static readonly Parser<char, string> DoubleQuotedStringParser = Parser.TakeWhile<char>(ch => ch != '\\' && ch != '"', min: 1)
            .Then(EscapeSequencesParser.Optional(Range.Failure))
            .Many()
            .Between(Parser.Is('"'))
            .Map(CopyToEscapedString)
            .WithName(nameof(DoubleQuotedStringParser));

        private static ParserPredicate<char> UnquotedArgumentPredicate = (data, index) => !char.IsWhiteSpace(data[index]) // No whitespace
            && "';{}\"".IndexOf(data[index]) < 0 // Illegal characters from RFC7950
            && (data[index] != '/' || (index + 1) >= data.Length || (data[index + 1] != '*' && data[index + 1] != '/')); // Respect line/block comments
         
        private static Parser<char, string> UnquotedArgumentParser = Parser.TakeWhile(UnquotedArgumentPredicate, 1)
            .Map(CopyToString)
            .WithName(nameof(UnquotedArgumentParser));

        private static readonly Parser<char, char> ConcatParser = Parser.Is('+').Between(OptionalSeparatorParser)
            .WithName(nameof(ConcatParser));

        private static readonly Parser<char, string> QuotedArgumentParser = DoubleQuotedStringParser.Or(SingleQuotedStringParser)
            .Then(DoubleQuotedStringParser.Or(SingleQuotedStringParser)
                .PrecededBy(ConcatParser)
                .Many())
            .Map((res, data) => {
                // If there are no concatenated strings just return the original
                if (!(res.Value.Item2.Count > 0))
                    return res.Value.Item1;

                var len = res.Value.Item1.Length;

                foreach (var str in res.Value.Item2)
                    len += str.Length;

                // Build a fixed-length string and copy the initial value, plus all concantenations into it.
                return StringUtilities.CreateString(len, res.Value, (span, state) => {
                    state.Item1.AsSpan().CopyTo(span);
                    span = span.Slice(state.Item1.Length);

                    foreach (var str in state.Item2) {
                        str.AsSpan().CopyTo(span);
                        span = span.Slice(str.Length);
                    }
                });
            })
            .WithName(nameof(QuotedArgumentParser));

        private static readonly Parser<char, string> ArgumentParser = QuotedArgumentParser
            .Or(UnquotedArgumentParser)
            .WithName(nameof(ArgumentParser));

        private static readonly Parser<char, Tuple<Tuple<string, string>, string>> StatementStartParser = KeywordParser
            .Then(ArgumentParser.PrecededBy(SeparatorParser))
            .WithName(nameof(StatementStartParser));

        // Unfortunately you cannot have a variable initialiser refer to itself in C#
        //  which means we cannot declare recursive parser definitions like this statically.
        //  Instead, we'll need to compose it manually and deal with the verbosity
        private static readonly Parser<char, YangStatement> SingleStatementParser = new Parser<char, YangStatement>(nameof(SingleStatementParser), (data, start, trace) => {
            var startResult = StatementStartParser.Parse(data, start, trace);
            var length = 0;

            if (!startResult.Success)
                return ParseResult.Fail<YangStatement>();

            length += startResult.Length;
            var keyword = startResult.Value.Item1;
            var argument = startResult.Value.Item2;
            var substatementResult = SubstatementParser.Parse(data, start + length, trace);

            return substatementResult.Success
                ? ParseResult.Success(new YangStatement(keyword.Item1, keyword.Item2, argument, substatementResult.Value), start, length + substatementResult.Length)
                : ParseResult.Fail<YangStatement>();
        });

        private static readonly Parser<char, YangStatement> StatementParser = SingleStatementParser
            .Between(OptionalSeparatorParser)
            .WithName(nameof(StatementParser));

        private static readonly Parser<char, IReadOnlyList<YangStatement>> SubstatementParser =
            Parser.Is(';').Map((r, d) => default(IReadOnlyList<YangStatement>))
                .Or(StatementParser.Many()
                    .Between(OptionalSeparatorParser) // account for empty { } block
                    .Between(Parser.Is('{'), Parser.Is('}')))
                .PrecededBy(OptionalSeparatorParser)
                .WithName(nameof(SubstatementParser));

        // --
        // Mapping helpers
        // --

        private static string CopyToString(ParseResult<Range> res, ReadOnlySpan<char> data) {
            return data.Slice(res.Value.Start, res.Value.Length).ToString();
        }

        private static string CopyToEscapedString(ParseResult<IReadOnlyList<Tuple<Range, Range>>> result, ReadOnlySpan<char> data) {
            // This map function is very complicated
            // It should handle all of the YANG escaping/indenting in double quoted string rules from https://tools.ietf.org/html/rfc7950
            // It should also perform as few allocations as possible; it is a relatively hot path in the parser
            var strlen = result.Length - 2; // Account for surrounding double quotes

            // Easy option 1) Empty string
            if (strlen == 0)
                return string.Empty;

            // Easy option 2) No escape characters and no newlines (ignoring starting/trailing double quotes
            if (result.Value[0].Item1.Length == strlen) {
                var slice = data.Slice(result.Value[0].Item1.Start, result.Value[0].Item1.Length);

                if (slice.IndexOf('\n') < 0)
                    return slice.ToString();
            }

            // Compute how indented the start of the string is, which will be used to offset the indentation of following lines
            // i.e
            //      "string starts here
            //          indents here"
            // The 2nd line should have a 4-space indent, not 8. Tabs are counted as 8 spaces as per the spec.
            var pos = result.Start + 1; // skip opening quote
            var origin = data.Slice(0, pos).LastIndexOf('\n') + 1; // Find the previous newline and start from there
            var tabcount = CountOccurences(data.Slice(origin, pos - origin), '\t');
            var indent = (pos - origin - tabcount) + (tabcount * 8); // Number of characters minus number of tabs, plus number of tabs * tabsize (8).

            // Build the escaped string
            // TODO: Maybe some analysis on how often the final StringBuilder.Length is < strlen
            var sb = new StringBuilder(strlen);

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
                // https://tools.ietf.org/html/rfc7950#section-6.1.3 trim trailing whitespace before each newline
                StringUtilities.Append(sb, str.Slice(0, ++newline));

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

            return StringUtilities.Append(sb, str);
        }

        private static StringBuilder AppendEscapedString(StringBuilder sb, ReadOnlySpan<char> str) {
            // Escape strings are always in the following format (see parser)
            // \x\y\z\a\b\c
            // where length is always a multiple of two and every odd index is a backslash.
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
                        sb.Append('\\');
                        sb.Append(str[i + 1]);
                        break;
                }
            }

            return sb;
        }
    }
}
