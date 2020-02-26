using System;
using System.Collections.Generic;
using System.Linq;

namespace Monda {
    public delegate ParseResult<TResult> ParseFunction<TSource, TResult>(ReadOnlySpan<TSource> data, int start, ParserTrace trace);
    public delegate bool ParserPredicate<TSource>(ReadOnlySpan<TSource> data, int index);

    public class Parser<TSource, TResult> {
        public string Name { get; private set; }

        private readonly ParseFunction<TSource, TResult> _parse;

        public Parser(ParseFunction<TSource, TResult> parse)
            : this(null, parse) { }

        public Parser(string name, ParseFunction<TSource, TResult> parse) {
            Name = name;
            _parse = parse;
        }

        public Parser<TSource, TResult> WithName(string name) {
            Name = name;
            return this;
        }

        public ParseResult<TResult> Parse(ReadOnlySpan<TSource> data) {
            return _parse(data, 0, null);
        }
      
        public ParseResult<TResult> Parse(ReadOnlySpan<TSource> data, int start) {
            return _parse(data, start, null);
        }

        public ParseResult<TResult> Parse(ReadOnlySpan<TSource> data, ParserTrace trace) {
            return Parse(data, 0, trace);
        }

        public ParseResult<TResult> Parse(ReadOnlySpan<TSource> data, int start, ParserTrace trace) {
            var result = _parse(data, start, trace);

            if (!result.Success && trace != null) {
                if (start > trace.Position) {
                    trace.Position = start;
                    trace.Parsers.Clear();
                }

                trace.Parsers.Add(Name ?? "(anonymous)");
            }

            return result;
        }
    }

    /// <summary>
    /// Static Parser helper methods
    /// </summary>
    public static class Parser {
        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that matches and yields a single <typeparamref name="TSource"/> element using the default comparer for <typeparamref name="TSource"/>
        /// </summary>
        /// <typeparam name="TSource">Type of the underlying data</typeparam>
        /// <param name="value">Element to match</param>
        /// <returns>A parser that will succeed if the <typeparamref name="TSource"/> element at the parser's current position equals <paramref name="value"/></returns>
        public static Parser<TSource, TSource> Is<TSource>(TSource value) {
            return Is(value, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that matches and yields a single <typeparamref name="TSource"/> element using a specified <see cref="IEqualityComparer{T}"/>
        /// </summary>
        /// <typeparam name="TSource">Type of the underlying data</typeparam>
        /// <param name="value">Element to compare</param>
        /// <param name="comparer">Comparer to use to determine equality between <typeparamref name="TSource"/> elements</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that will succeed if the <typeparamref name="TSource"/> element at the parser's current position equals <paramref name="value"/></returns>
        public static Parser<TSource, TSource> Is<TSource>(TSource value, IEqualityComparer<TSource> comparer) {
            return new Parser<TSource, TSource>((data, start, trace) => {
                return start < data.Length && comparer.Equals(data[start], value)
                    ? ParseResult.Success(value, start, 1)
                    : ParseResult.Fail<TSource>();
            });
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that matches and yields a sequence of <typeparamref name="TSource"/> elements using the default comparer for <typeparamref name="TSource"/>
        /// </summary>
        /// <typeparam name="TSource">Type of the underlying data</typeparam>
        /// <param name="value"><see cref="ReadOnlyMemory{T}"/> containing the sequence of <typeparamref name="TSource"/> elements to compare</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that will succeed if the sequence of <typeparamref name="TSource"/> elements at the parser's current position equals <paramref name="value"/></returns>
        public static Parser<TSource, IReadOnlyList<TSource>> IsSequence<TSource>(IReadOnlyList<TSource> value) {
            return IsSequence(value, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that matches and yields a sequence of <typeparamref name="TSource"/> elements using the specified comparer
        /// </summary>
        /// <typeparam name="TSource">Type of the underlying data</typeparam>
        /// <param name="value"><see cref="ReadOnlyMemory{T}"/> containing the sequence of <typeparamref name="TSource"/> elements to compare</param>
        /// <param name="comparer">Comparer to use to determine equality between <typeparamref name="TSource"/> elements</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that will succeed if the sequence of <typeparamref name="TSource"/> elements at the parser's current position equals <paramref name="value"/></returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is empty</exception>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is null</exception>
        public static Parser<TSource, IReadOnlyList<TSource>> IsSequence<TSource>(IReadOnlyList<TSource> value, IEqualityComparer<TSource> comparer) {
            if (value == null || comparer == null)
                throw new ArgumentNullException(value == null ? nameof(value) : nameof(comparer));

            if (value.Count == 0)
                throw new ArgumentException("input list is empty", nameof(value));

            return new Parser<TSource, IReadOnlyList<TSource>>((data, start, trace) => {
                if ((data.Length - start) < value.Count)
                    return ParseResult.Fail<IReadOnlyList<TSource>>();

                for (var i = 0; i < value.Count; ++i) {
                    if (!comparer.Equals(data[start + i], value[i]))
                        return ParseResult.Fail<IReadOnlyList<TSource>>();
                }

                return ParseResult.Success(value, start, value.Count);
            });
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that matches and yields the inverse of single <typeparamref name="TSource"/> element using a specified <see cref="IEqualityComparer{T}"/>
        /// </summary>
        /// <typeparam name="TSource">Type of the underlying data</typeparam>
        /// <param name="value">Element to compare</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that yields the <typeparamref name="TSource"/> element at the parser's current position if it does not equal <paramref name="value"/></returns>
        public static Parser<TSource, TSource> IsNot<TSource>(TSource value) {
            return IsNot(value, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that matches and yields the inverse of single <typeparamref name="TSource"/> element using a specified <see cref="IEqualityComparer{T}"/>
        /// </summary>
        /// <typeparam name="TSource">Type of the underlying data</typeparam>
        /// <param name="value">Element to compare</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that yields the <typeparamref name="TSource"/> element at the parser's current position if it does not equal <paramref name="value"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is null</exception>
        public static Parser<TSource, TSource> IsNot<TSource>(TSource value, IEqualityComparer<TSource> comparer) {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new Parser<TSource, TSource>((data, start, trace) => {
                return start < data.Length && !comparer.Equals(data[start], value)
                    ? ParseResult.Success(value, start, 1)
                    : ParseResult.Fail<TSource>();
            });
        }

        public static Parser<TSource, TSource> IsAny<TSource>(IReadOnlyList<TSource> value) {
            return IsAny(value, EqualityComparer<TSource>.Default);
        }

        public static Parser<TSource, TSource> IsAny<TSource>(IReadOnlyList<TSource> values, IEqualityComparer<TSource> comparer) {
            if (values == null || comparer == null)
                throw new ArgumentNullException(values == null ? nameof(values) : nameof(comparer));

            if (values.Count == 0)
                throw new ArgumentException("input list is empty", nameof(values));

            return new Parser<TSource, TSource>((data, start, trace) => {
                return start < data.Length && values.Contains(data[start], comparer)
                    ? ParseResult.Success(data[start], start, 1)
                    : ParseResult.Fail<TSource>();
            });
        }

        public static Parser<TSource, TSource> IsNotAny<TSource>(IReadOnlyList<TSource> values) {
            return IsNotAny(values, EqualityComparer<TSource>.Default);
        }

        public static Parser<TSource, TSource> IsNotAny<TSource>(IReadOnlyList<TSource> values, IEqualityComparer<TSource> comparer) {
            if (values == null || comparer == null)
                throw new ArgumentNullException(values == null ? nameof(values) : nameof(comparer));

            if (values.Count == 0)
                throw new ArgumentException("input list is empty", nameof(values));

            return new Parser<TSource, TSource>((data, start, trace) => {
                return start < data.Length && !values.Contains(data[start], comparer)
                    ? ParseResult.Success(data[start], start, 1)
                    : ParseResult.Fail<TSource>();
            });
        }

        public static Parser<TSource, Range> TakeWhile<TSource>(Func<TSource, bool> predicate, int min = 0, int? max = default) {
            return TakeWhile<TSource>((data, index) => predicate(data[index]), min, max);
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}" /> that matches a range of <typeparamref name="TSource"/> elements against a predicate
        /// </summary>
        /// <typeparam name="TSource">Type of the underlying data</typeparam>
        /// <param name="predicate">Predicate that is invoked for each <typeparamref name="TSource"/> element</param>
        /// <param name="min">Minumum number of matches required (inclusive)</param>
        /// <param name="max">Maximum number of matches allowed (inclusive)</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that yields the <see cref="Range"/> of data that was matched</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is less than 0 or <paramref name="max"/> is less than <paramref name="min"/></exception>
        public static Parser<TSource, Range> TakeWhile<TSource>(ParserPredicate<TSource> predicate, int min = 0, int? max = default) {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (min < 0 || max < min)
                throw new ArgumentOutOfRangeException(min < 0 ? nameof(min) : nameof(max));

            return new Parser<TSource, Range>((data, start, trace) => {
                var len = 0;
                var local = data.Slice(start);

                while (len < local.Length && (max == default || len < max) && predicate(local, len))
                    ++len;
      
                return len < min
                    ? ParseResult.Fail(Range.Failure)
                    : ParseResult.Success(new Range(start, len), start, len);
            });
        }


        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that matches any <typeparamref name="TSource"/> element until the <paramref name="next"/> parser succeeds
        /// </summary>
        /// <typeparam name="TSource">The type of the underlying data</typeparam>
        /// <typeparam name="TNext">The return type of the next parser</typeparam>
        /// <param name="next">A </param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static Parser<TSource, Tuple<Range, TNext>> TakeUntil<TSource, TNext>(Parser<TSource, TNext> next, int min = 0) {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min));

            return new Parser<TSource, Tuple<Range, TNext>>((data, start, trace) => {
                var pos = start;

                while (pos < data.Length) {
                    var res = next.Parse(data, pos, trace);

                    if (res.Success && (pos - start) >= min) 
                        return ParseResult.Success(Tuple.Create(new Range(start, pos - start), res.Value), start, (pos - start) + res.Length);
                
                    ++pos;
                }

                return ParseResult.Fail<Tuple<Range, TNext>>();
            });
        }
    }
}