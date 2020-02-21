using System;
using System.Collections.Generic;

namespace Monda {
    public delegate ParseResult<TResult> ParseFunction<TSource, TResult>(ReadOnlySpan<TSource> data, int start);

    public class Parser<TSource, TResult> {
        public string Name { get; }

        private readonly ParseFunction<TSource, TResult> _parse;

        public Parser(ParseFunction<TSource, TResult> parse)
            : this(null, parse) { }

        public Parser(string name, ParseFunction<TSource, TResult> parse) {
            Name = name;
            _parse = parse;
        }

        public ParseResult<TResult> Parse(ReadOnlySpan<TSource> data) {
            return _parse(data, 0);
        }
      
        public ParseResult<TResult> Parse(ReadOnlySpan<TSource> data, int start) {
            return _parse(data, start);
        }
    }

    /// <summary>
    /// Static Parser helper methods
    /// </summary>
    public static class Parser {
        public static Parser<TSource, TSource> Is<TSource>(TSource value) where TSource : IEquatable<TSource> {
            return new Parser<TSource, TSource>((data, start) => {
                return start < data.Length && data[start].Equals(value)
                    ? ParseResult.Success(value, start, 1)
                    : ParseResult.Fail<TSource>();
            });
        }

        public static Parser<TSource, TSource> IsNot<TSource>(TSource value) where TSource : IEquatable<TSource> {
            return new Parser<TSource, TSource>((data, start) => {
                return start < data.Length && !data[start].Equals(value)
                    ? ParseResult.Success(value, start, 1)
                    : ParseResult.Fail<TSource>();
            });
        }

        public static Parser<TSource, ReadOnlyMemory<TSource>> Literal<TSource>(ReadOnlyMemory<TSource> value) {
            return Literal(value, EqualityComparer<TSource>.Default);
        }

        public static Parser<TSource, ReadOnlyMemory<TSource>> Literal<TSource>(ReadOnlyMemory<TSource> value, IEqualityComparer<TSource> comparer) {
            return new Parser<TSource, ReadOnlyMemory<TSource>>((data, start) => {
                if ((data.Length - start) < value.Length)
                    return ParseResult.Fail<ReadOnlyMemory<TSource>>();

                var span = value.Span;

                for (var i = 0; i < span.Length; ++i) {
                    if (comparer.Equals(data[start + i], span[i]))
                        return ParseResult.Fail<ReadOnlyMemory<TSource>>();
                }

                return ParseResult.Success(value, start, value.Length);
            });
        }

        public static Parser<TSource, Range> Match<TSource>(Func<TSource, bool> test, int min = 0, int? max = default) {
            if (test == null)
                throw new ArgumentNullException(nameof(test));
            if (min < 0 || max < min)
                throw new ArgumentOutOfRangeException(min < 0 ? nameof(min) : nameof(max));

            return new Parser<TSource, Range>((data, start) => {
                var pos = start;
                var len = 0;

                while (pos < data.Length && (max == default || len < max) && test(data[pos])) {
                    ++pos;
                    ++len;
                }

                return len < min
                    ? ParseResult.Fail(Range.Failure)
                    : ParseResult.Success(new Range(start, len), start, len);
            });
        }

        public static Parser<TSource, Tuple<Range, TNext>> MatchUntil<TSource, TNext>(Func<TSource, bool> test, Parser<TSource, TNext> next, int min = 0, int? max = default) {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            if (min < 0 || max < min)
                throw new ArgumentOutOfRangeException(min < 0 ? nameof(min) : nameof(max));

            return new Parser<TSource, Tuple<Range, TNext>>((data, start) => {
                var pos = start;
                var len = 0;
                var nextValue = default(TNext);

                while (pos < data.Length && (max == default || len < max)) {
                    var res = next.Parse(data, pos);

                    if (res.Success) {
                        nextValue = res.Value;
                        pos += res.Length;
                        break;
                    }

                    if (!test(data[pos]))
                        break;

                    ++pos;
                    ++len;
                }

                return len < min
                    ? ParseResult.Fail<Tuple<Range, TNext>>()
                    : ParseResult.Success(Tuple.Create(new Range(start, len), nextValue), start, pos - start);
            });
        }
    }
}