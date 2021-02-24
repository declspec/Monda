using System;
using System.Collections.Generic;
using System.Linq;

namespace Monda {
    /// <summary>
    /// Delegate that attempts to parse a result from an input data set, beginning at a specific point
    /// </summary>
    /// <typeparam name="TSource">The type of items in the input data</typeparam>
    /// <typeparam name="TResult">The result type of the parser</typeparam>
    /// <param name="data">Input data to parse</param>
    /// <param name="start">Initial position to begin parsing from</param>
    /// <param name="trace">A <see cref="ParserTrace"/> which can be passed to other parser functions to assist in debugging</param>
    /// <returns>A <see cref="ParseResult{TValue}"/> indicating the success or failure of the parse attempt</returns>
    public delegate ParseResult<TResult> ParseFunction<TSource, TResult>(ReadOnlySpan<TSource> data, int start, ParserTrace trace);

    /// <summary>
    /// Predicate that tests the <typeparamref name="TSource"/> item at the current index.
    /// </summary>
    /// <typeparam name="TSource">The type of items in the input data</typeparam>
    /// <param name="data">Input data to parse</param>
    /// <param name="index">Index of the current item in <paramref name="data" /> to test</param>
    /// <returns>True if the <typeparamref name="TSource"/> item at the current index is acceptable, false otherwise</returns>
    public delegate bool ParserPredicate<TSource>(ReadOnlySpan<TSource> data, int index);

    /// <summary>
    /// Fundamental parsing unit to extract one <typeparamref name="TResult"/> from a set of <typeparamref name="TSource"/> input items
    /// </summary>
    /// <typeparam name="TSource">The type of items in the input data</typeparam>
    /// <typeparam name="TResult">The result type of the parser</typeparam>
    public class Parser<TSource, TResult> {
        /// <summary>
        /// Identifying name for the parser
        /// </summary>
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
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a single <typeparamref name="TSource"/> item if it equals <paramref name="value"/>, using the default comparer for <typeparamref name="TSource"/> to compare items
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="value">Item to match</param>
        /// <returns>A parser that will succeed if the <typeparamref name="TSource"/> item at the parser's current position equals <paramref name="value"/></returns>
        public static Parser<TSource, TSource> Is<TSource>(TSource value) {
            return Is(value, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a single <typeparamref name="TSource"/> item if it equals <paramref name="value"/>, using the <paramref name="comparer"/> to compare items.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="value">Item to compare</param>
        /// <param name="comparer">Comparer used to determine equality between <typeparamref name="TSource"/> items</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that will succeed if the <typeparamref name="TSource"/> item at the parser's current position equals <paramref name="value"/></returns>
        public static Parser<TSource, TSource> Is<TSource>(TSource value, IEqualityComparer<TSource> comparer) {
            return new Parser<TSource, TSource>((data, start, trace) => {
                return start < data.Length && comparer.Equals(data[start], value)
                    ? ParseResult.Success(value, start, 1)
                    : ParseResult.Fail<TSource>();
            });
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a single <typeparamref name="TSource"/> item if it passes <paramref name="predicate"/>
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="predicate">Predicate that is invoked against the next <typeparamref name="TSource"/> item</param>
        /// <returns>A parser that will succeed if the <typeparamref name="TSource"/> item at the parser's current position passes <paramref name="predicate"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null</exception>
        public static Parser<TSource, TSource> Is<TSource>(Func<TSource, bool> predicate) {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new Parser<TSource, TSource>((data, start, trace) => {
                return start < data.Length && predicate(data[start])
                    ? ParseResult.Success(data[start], start, 1)
                    : ParseResult.Fail<TSource>();
            });
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a single <typeparamref name="TSource"/> item if it does not equal <paramref name="value"/>, using the default comparer for <typeparamref name="TSource"/> to compare items
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="value">Item to match</param>
        /// <returns>A parser that will succeed if the <typeparamref name="TSource"/> item at the parser's current position does not equal <paramref name="value"/></returns>
        public static Parser<TSource, TSource> IsNot<TSource>(TSource value) {
            return IsNot(value, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a single <typeparamref name="TSource"/> item if it does not equal <paramref name="value"/>, using <paramref name="comparer"/> to compare items
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="value">Item to match</param>
        /// <param name="comparer">Comparer used to determine equality between <typeparamref name="TSource"/> items</param>
        /// <returns>A parser that will succeed if the <typeparamref name="TSource"/> item at the parser's current position does not equal <paramref name="value"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is null</exception>
        public static Parser<TSource, TSource> IsNot<TSource>(TSource value, IEqualityComparer<TSource> comparer) {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new Parser<TSource, TSource>((data, start, trace) => {
                return start < data.Length && !comparer.Equals(data[start], value)
                    ? ParseResult.Success(data[start], start, 1)
                    : ParseResult.Fail<TSource>();
            });
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a single <typeparamref name="TSource"/> item if it does not pass <paramref name="predicate"/>
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="predicate">Predicate that is invoked against the next <typeparamref name="TSource"/> item</param>
        /// <returns>A parser that will succeed if the <typeparamref name="TSource"/> item at the parser's current position does not pass <paramref name="predicate"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null</exception>
        public static Parser<TSource, TSource> IsNot<TSource>(Func<TSource, bool> predicate) {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new Parser<TSource, TSource>((data, start, trace) => {
                return start < data.Length && !predicate(data[start])
                    ? ParseResult.Success(data[start], start, 1)
                    : ParseResult.Fail<TSource>();
            });
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a <see cref="IReadOnlyList{TSource}"/> if the sequence of items at the current position equals <paramref name="value"/>, using the default comparer for <typeparamref name="TSource"/> to compare items
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="value"><see cref="ReadOnlyMemory{T}"/> containing the sequence of <typeparamref name="TSource"/> items to compare</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that will succeed if the sequence of <typeparamref name="TSource"/> items at the parser's current position equals <paramref name="value"/></returns>
        public static Parser<TSource, IReadOnlyList<TSource>> IsSequence<TSource>(IReadOnlyList<TSource> value) {
            return IsSequence(value, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a <see cref="IReadOnlyList{TSource}"/> if the sequence of items at the current position equals <paramref name="value"/>, using <paramref name="comparer"/> to compare items
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="value"><see cref="ReadOnlyMemory{T}"/> containing the sequence of <typeparamref name="TSource"/> items to compare</param>
        /// <param name="comparer">Comparer used to determine equality between <typeparamref name="TSource"/> items</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that will succeed if the sequence of <typeparamref name="TSource"/> items at the parser's current position equals <paramref name="value"/></returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is empty</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> or <paramref name="comparer"/> is null</exception>
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
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a single <typeparamref name="TSource"/> item if it exists in <paramref name="values"/>, using the default comparer for <typeparamref name="TSource"/> to compare items
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="values">Item to compare</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that will succeed if the <typeparamref name="TSource"/> item at the parser's current position exists in <paramref name="values"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null</exception>
        public static Parser<TSource, TSource> IsAny<TSource>(IReadOnlyList<TSource> values) {
            return IsAny(values, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a single <typeparamref name="TSource"/> item if it exists in <paramref name="values"/>, using <paramref name="comparer"/> to compare values
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="values">Item to compare</param>
        /// <param name="comparer">Comparer to use to determine equality between <typeparamref name="TSource"/> items</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that will succeed if the <typeparamref name="TSource"/> item at the parser's current position exists in <paramref name="values"/></returns>
        /// <exception cref="ArgumentException"><paramref name="values"/> is empty</exception>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> or <paramref name="comparer"/> is null</exception>
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

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a single <typeparamref name="TSource"/> item if it does not exist in <paramref name="values"/>, using the default comparer for <typeparamref name="TSource"/> to compare items
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="values">Item to compare</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that will succeed if the <typeparamref name="TSource"/> item at the parser's current position does not exist in <paramref name="values"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null</exception>
        public static Parser<TSource, TSource> IsNotAny<TSource>(IReadOnlyList<TSource> values) {
            return IsNotAny(values, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}"/> that yields a single <typeparamref name="TSource"/> item if it does not exist in <paramref name="values"/>, using <paramref name="comparer"/> to compare values
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="values">Item to compare</param>
        /// <param name="comparer">Comparer to use to determine equality between <typeparamref name="TSource"/> items</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that will succeed if the <typeparamref name="TSource"/> item at the parser's current position does not exist in <paramref name="values"/></returns>
        /// <exception cref="ArgumentException"><paramref name="values"/> is empty</exception>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> or <paramref name="comparer"/> is null</exception>
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

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}" /> that yields the range of <typeparamref name="TSource"/> items from the current parser position matching <paramref name="predicate"/>
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="predicate">Predicate that is invoked for each <typeparamref name="TSource"/> item</param>
        /// <param name="min">Minimum number of matches required (inclusive)</param>
        /// <param name="max">Maximum number of matches allowed (inclusive)</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that yields the <see cref="Range"/> of data that was matched</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is less than 0 or <paramref name="max"/> is less than <paramref name="min"/></exception>
        public static Parser<TSource, Range> TakeWhile<TSource>(Func<TSource, bool> predicate, int min = 0, int? max = default) {
            return TakeWhile<TSource>((data, index) => predicate(data[index]), min, max);
        }

        /// <summary>
        /// Creates a <see cref="Parser{TSource, TResult}" /> that yields the range of <typeparamref name="TSource"/> items from the current parser position matching <paramref name="predicate"/>
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <param name="predicate">Predicate that is invoked for each <typeparamref name="TSource"/> item</param>
        /// <param name="min">Minimum number of matches required (inclusive)</param>
        /// <param name="max">Maximum number of matches allowed (inclusive)</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that yields the <see cref="Range"/> of data that was matched</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is less than 0 or <paramref name="max"/> is less than <paramref name="min"/></exception>
        /// <remarks>The <see cref="ReadOnlySpan{TSource}"/> passed to <paramref name="predicate"/> is a slice starting at the parser's intial position and the index will increase from 0</remarks>
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
        /// Creates a <see cref="Parser{TSource, TResult}" /> that yields the range of <typeparamref name="TSource"/> items from the current parser until <paramref name="next"/> succeeds
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TNext">The return type of the next parser</typeparam>
        /// <param name="next">A <see cref="Parser{TSource, TNext}"/> that will succeed at the end of the range</param>
        /// <param name="min">Minimum number of matches required (inclusive)</param>
        /// <returns>A <see cref="Parser{TSource, TResult}"/> that yields a tuple containing the <see cref="Range" /> that was matched and the result of <paramref name="next"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="next"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is less than 0</exception>
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