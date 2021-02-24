using System;
using System.Collections.Generic;

namespace Monda {
    public delegate TOut MapFunction<TSource, TIn, TOut>(ParseResult<TIn> result, ReadOnlySpan<TSource> data);
    public delegate bool TryMapFunction<TSource, TIn, TOut>(ParseResult<TIn> result, ReadOnlySpan<TSource> data, out TOut mapped);

    public static class ParserExtensions {
        /// <summary>
        /// Create a new parser that executes the current parser and <paramref name="next"/> in order and combines the results
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of the current parser</typeparam>
        /// <typeparam name="TNext">The result type of <paramref name="next"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="next">The next <see cref="Parser{TSource, TResult}"/></param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes <paramref name="self"/> and <paramref name="next"/> is sequence and returns a <see cref="Tuple{TResult,TNext}"/> of the results if both are successful</returns>
        public static Parser<TSource, Tuple<TResult, TNext>> Then<TSource, TResult, TNext>(this Parser<TSource, TResult> self, Parser<TSource, TNext> next) {
            if (self == null || next == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(next));

            return new Parser<TSource, Tuple<TResult, TNext>>((data, start, trace) => {
                var selfRes = self.Parse(data, start, trace);

                if (!selfRes.Success)
                    return ParseResult.Fail<Tuple<TResult, TNext>>();

                var nextRes = next.Parse(data, start + selfRes.Length, trace);

                return nextRes.Success
                    ? ParseResult.Success(Tuple.Create(selfRes.Value, nextRes.Value), start, selfRes.Length + nextRes.Length)
                    : ParseResult.Fail<Tuple<TResult, TNext>>();
            });
        }

        /// <summary>
        /// Create a new parser that ensures that the current parser is preceded by <paramref name="previous"/>
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <typeparam name="TPrevious">The result type of <paramref name="previous"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="previous">The parser to execute before the current parser, its result will be discarded</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes <paramref name="previous"/> and <paramref name="self"/> in that order and returns the <typeparamref name="TResult"/> from <paramref name="self"/></returns>
        /// <remarks>Although the parsing position will be advanced accordingly, the result of <paramref name="previous"/> will be discarded. Use <see cref="Then{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext})"/> if you need to capture the result</remarks>
        public static Parser<TSource, TResult> PrecededBy<TSource, TResult, TPrevious>(this Parser<TSource, TResult> self, Parser<TSource, TPrevious> previous) {
            if (self == null || previous == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(previous));

            return new Parser<TSource, TResult>((data, start, trace) => {
                var previousRes = previous.Parse(data, start, trace);

                if (!previousRes.Success)
                    return ParseResult.Fail<TResult>();

                var selfRes = self.Parse(data, start + previousRes.Length, trace);

                return selfRes.Success
                    ? ParseResult.Success(selfRes.Value, start, previousRes.Length + selfRes.Length)
                    : ParseResult.Fail<TResult>();
            });
        }

        /// <summary>
        /// Create a new parser that ensures that the current parser is followed by <paramref name="next"/>
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <typeparam name="TNext">The result type of <paramref name="next"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="next">The parser to execute after the current parser, its result will be discarded</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes <paramref name="self"/> and <paramref name="next"/> in that order and returns the <typeparamref name="TResult"/> from <paramref name="self"/></returns>
        /// <remarks>Although the parsing position will be advanced accordingly, the result of <paramref name="next"/> will be discarded. Use <see cref="Then{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext})"/> if you need to capture the result</remarks>
        public static Parser<TSource, TResult> FollowedBy<TSource, TResult, TNext>(this Parser<TSource, TResult> self, Parser<TSource, TNext> next) {
            if (self == null || next == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(next));

            return new Parser<TSource, TResult>((data, start, trace) => {
                var selfRes = self.Parse(data, start, trace);

                if (!selfRes.Success)
                    return selfRes;

                var nextRes = next.Parse(data, start + selfRes.Length, trace);

                return nextRes.Success 
                    ? ParseResult.Success(selfRes.Value, start, selfRes.Length + nextRes.Length) 
                    : ParseResult.Fail<TResult>();
            });
        }

        /// <summary>
        /// Create a new parser that executes the current parser, or <paramref name="other"/> if the current parser fails
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/> and <paramref name="other"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="other">The other parser to execute if the current one is fails</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that returns the first successful result of the current parser and <paramref name="other"/> in that order</returns>
        public static Parser<TSource, TResult> Or<TSource, TResult>(this Parser<TSource, TResult> self, Parser<TSource, TResult> other) {
            if (self == null || other == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(other));

            return new Parser<TSource, TResult>((data, start, trace) => {
                var res = self.Parse(data, start, trace);
                return res.Success ? res : other.Parse(data, start, trace);
            });
        }

        /// <summary>
        /// Creates a new parser that ensures that the current parser is surrounded by <paramref name="other"/>
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <typeparam name="TOther">The result type of <paramref name="other"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="other">The parser to match before and after the current parser</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that returns the result of the current parser if it is found between <paramref name="other" /></returns>
        public static Parser<TSource, TResult> Between<TSource, TResult, TOther>(this Parser<TSource, TResult> self, Parser<TSource, TOther> other) {
            return Between(self, other, other);
        }

        /// <summary>
        /// Creates a new parser that ensures that the current parser is between <paramref name="left"/> and <paramref name="right"/>
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <typeparam name="TLeft">The result type of <paramref name="left"/></typeparam>
        /// <typeparam name="TRight">The result type of <paramref name="right"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="left">The first parser to be executed</param>
        /// <param name="right">The last parser to execute</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that returns the result of the current parser if it is found between <paramref name="left"/> and <paramref name="right"/></returns>
        public static Parser<TSource, TResult> Between<TSource, TResult, TLeft, TRight>(this Parser<TSource, TResult> self, Parser<TSource, TLeft> left, Parser<TSource, TRight> right) {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            if (left == null || right == null)
                throw new ArgumentNullException(left == null ? nameof(left) : nameof(right));

            return new Parser<TSource, TResult>((data, start, trace) => {
                var leftRes = left.Parse(data, start, trace);
                var length = 0;

                if (!leftRes.Success)
                    return ParseResult.Fail<TResult>();

                length += leftRes.Length;
                var selfRes = self.Parse(data, start + length, trace);

                if (!selfRes.Success)
                    return ParseResult.Fail<TResult>();

                length += selfRes.Length;
                var rightRes = right.Parse(data, start + length, trace);

                if (!rightRes.Success)
                    return ParseResult.Fail<TResult>();

                length += rightRes.Length;
                return ParseResult.Success(selfRes.Value, start, length);
            });
        }

        /// <summary>
        /// Create a new parser that maps the result of the current parser to a new value
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <typeparam name="TNext">The mapped value type</typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="map">A function that maps the value from the current parser to a new value</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes the current parser and maps the value to a new value</returns>
        public static Parser<TSource, TNext> Map<TSource, TResult, TNext>(this Parser<TSource, TResult> self, MapFunction<TSource, TResult, TNext> map) {
            return TryMap(self, (ParseResult<TResult> res, ReadOnlySpan<TSource> data, out TNext next) => {
                next = map(res, data);
                return true;
            });
        }

        /// <summary>
        /// Create a new parser that tries to map the result of the current parser to a new value
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <typeparam name="TNext">The mapped value type</typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="tryMap">A function that tries to map the value from the current parser to a new value</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes the current parser and tries to map the value to a new value</returns>
        public static Parser<TSource, TNext> TryMap<TSource, TResult, TNext>(this Parser<TSource, TResult> self, TryMapFunction<TSource, TResult, TNext> tryMap) {
            if (self == null || tryMap == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(tryMap));

            return new Parser<TSource, TNext>((data, start, trace) => {
                var res = self.Parse(data, start, trace);
                return res.Success && tryMap(res, data, out var mapped)
                    ? ParseResult.Success(mapped, res.Start, res.Length)
                    : ParseResult.Fail<TNext>();
            });
        }

        /// <summary>
        /// Create a new parser that executes the current parser a fixed number of times and aggregates the results
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="count">The number of times to repeat the current parser</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes the current parser <paramref name="count"/> times and aggregates the results</returns>
        /// <exception cref="StackOverflowException">If the current parser returns a zero-length result</exception>
        /// <seealso cref="Skip{TSource, TResult}(Parser{TSource, TResult}, int)"/>
        /// <seealso cref="Many{TSource, TResult}(Parser{TSource, TResult}, int, int?)"/>
        /// <seealso cref="ManyUntil{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext}, int)"/>
        /// <seealso cref="SkipMany{TSource, TResult}(Parser{TSource, TResult}, int, int?)"/>
        /// <seealso cref="SkipUntil{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext}, int)"/>
        public static Parser<TSource, IReadOnlyList<TResult>> Repeat<TSource, TResult>(this Parser<TSource, TResult> self, int count) {
            return Many(self, count, count);
        }

        /// <summary>
        /// Create a new parser that executes the current parser until failure or an upper bound is reached and aggregates the results
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="min">The minimum number of times the parser must execute (inclusive)</param>
        /// <param name="max">The maximum number of times the parser can execute (inclusive). A null value indicates there is no upper bound</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes the current parser between <paramref name="min"/> and <paramref name="max"/> times and aggregates the results</returns>
        /// <exception cref="StackOverflowException">If the current parser returns a zero-length result</exception>
        /// <seealso cref="Repeat{TSource, TResult}(Parser{TSource, TResult}, int)"/>
        /// <seealso cref="ManyUntil{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext}, int)"/>
        /// <seealso cref="Skip{TSource, TResult}(Parser{TSource, TResult}, int)"/>
        /// <seealso cref="SkipMany{TSource, TResult}(Parser{TSource, TResult}, int, int?)"/>
        /// <seealso cref="SkipUntil{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext}, int)"/>
        public static Parser<TSource, IReadOnlyList<TResult>> Many<TSource, TResult>(this Parser<TSource, TResult> self, int min = 0, int? max = default) {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min));

            if (max.HasValue && max < min)
                throw new ArgumentOutOfRangeException(nameof(max));

            return new Parser<TSource, IReadOnlyList<TResult>>((data, start, trace) => {
                var values = default(List<TResult>);
                var length = 0;

                while (max == default || values.Count < max) {
                    var res = self.Parse(data, start + length, trace);

                    if (!res.Success)
                        break;

                    if (res.Length == 0)
                        ThrowOverflow(nameof(Many));

                    if (values == null)
                        values = new List<TResult>();

                    values.Add(res.Value);
                    length += res.Length;
                }

                return min == 0 || values?.Count >= min
                    ? ParseResult.Success((IReadOnlyList<TResult>)values ?? Array.Empty<TResult>(), start, length)
                    : ParseResult.Fail<IReadOnlyList<TResult>>();
            });
        }

        /// <summary>
        /// Create a new parser that executes the current parser until another parser succeeds, then aggregates and combines the results
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <typeparam name="TNext">The result type of <paramref name="next"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="next">The parser that determines when to stop repeating</param>
        /// <param name="min">The minimum number of times the parser must execute</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes the current parser until <paramref name="next"/> succeeds, aggregating and combining the results</returns>
        /// <exception cref="StackOverflowException">If the current parser returns a zero-length result</exception>
        /// <seealso cref="Repeat{TSource, TResult}(Parser{TSource, TResult}, int)"/>
        /// <seealso cref="Many{TSource, TResult}(Parser{TSource, TResult}, int, int?)"/>
        /// <seealso cref="Skip{TSource, TResult}(Parser{TSource, TResult}, int)"/>
        /// <seealso cref="SkipMany{TSource, TResult}(Parser{TSource, TResult}, int, int?)"/>
        /// <seealso cref="SkipUntil{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext}, int)"/>
        public static Parser<TSource, Tuple<IReadOnlyList<TResult>, TNext>> ManyUntil<TSource, TResult, TNext>(this Parser<TSource, TResult> self, Parser<TSource, TNext> next, int min = 0) {
            if (self == null || next == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(next));

            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min));

            return new Parser<TSource, Tuple<IReadOnlyList<TResult>, TNext>>((data, start, trace) => {
                var values = default(List<TResult>);
                var length = 0;

                while (true) {
                    var nextRes = next.Parse(data, start + length, trace);

                    if (nextRes.Success) {
                        return min == 0 || values?.Count >= min
                            ? ParseResult.Success(Tuple.Create((IReadOnlyList<TResult>)values ?? Array.Empty<TResult>(), nextRes.Value), start, length + nextRes.Length)
                            : ParseResult.Fail<Tuple<IReadOnlyList<TResult>, TNext>>();
                    }
                    
                    var res = self.Parse(data, start + length, trace);

                    if (!res.Success)
                        return ParseResult.Fail<Tuple<IReadOnlyList<TResult>, TNext>>();

                    if (res.Length == 0)
                        ThrowOverflow(nameof(ManyUntil));

                    if (values == null)
                        values = new List<TResult>();

                    values.Add(res.Value);
                    length += res.Length;
                }
            });
        }

        /// <summary>
        /// Create a new parser that executes the current parser a fixed number of times and counts the number of results
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="count">The number of times to repeat the current parser</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes the current parser <paramref name="count"/> times, counting the number of results</returns>
        /// <exception cref="StackOverflowException">If the current parser returns a zero-length result</exception>
        /// <seealso cref="Repeat{TSource, TResult}(Parser{TSource, TResult}, int)"/>
        /// <seealso cref="Many{TSource, TResult}(Parser{TSource, TResult}, int, int?)"/>
        /// <seealso cref="ManyUntil{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext}, int)"/>
        /// <seealso cref="SkipMany{TSource, TResult}(Parser{TSource, TResult}, int, int?)"/>
        /// <seealso cref="SkipUntil{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext}, int)"/>
        public static Parser<TSource, int> Skip<TSource, TResult>(this Parser<TSource, TResult> self, int count) {
            return SkipMany(self, count, count);
        }

        /// <summary>
        /// Create a new parser that executes the current parser until failure or an upper bound is reached and counts the number of results
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="min">The minimum number of times the parser must execute (inclusive)</param>
        /// <param name="max">The maximum number of times the parser can execute (inclusive). A null value indicates there is no upper bound</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes the current parser between <paramref name="min"/> and <paramref name="max"/> times and counts the number results</returns>
        /// <exception cref="StackOverflowException">If the current parser returns a zero-length result</exception>
        /// <seealso cref="Repeat{TSource, TResult}(Parser{TSource, TResult}, int)"/>
        /// <seealso cref="Many{TSource, TResult}(Parser{TSource, TResult}, int, int?)"/>
        /// <seealso cref="ManyUntil{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext}, int)"/>
        /// <seealso cref="Skip{TSource, TResult}(Parser{TSource, TResult}, int)"/>
        /// <seealso cref="SkipUntil{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext}, int)"/>
        public static Parser<TSource, int> SkipMany<TSource, TResult>(this Parser<TSource, TResult> self, int min = 0, int? max = default) {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min));

            if (max.HasValue && max < min)
                throw new ArgumentOutOfRangeException(nameof(max));

            return new Parser<TSource, int>((data, start, trace) => {
                var skipped = 0;
                var length = 0;

                while (max == default || skipped < max) {
                    var res = self.Parse(data, start + length, trace);

                    if (!res.Success)
                        break;

                    if (res.Length == 0)
                        ThrowOverflow(nameof(SkipMany));

                    length += res.Length;
                    ++skipped;   
                }

                return skipped >= min
                    ? ParseResult.Success(skipped, start, length)
                    : ParseResult.Fail<int>();
            });
        }

        /// <summary>
        /// Create a new parser that executes the current parser until another parser succeeds, then counts and combines the results
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <typeparam name="TNext">The result type of <paramref name="next"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="next">The parser that determines when to stop repeating</param>
        /// <param name="min">The minimum number of times the parser must execute</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes the current parser until <paramref name="next"/> succeeds, counting and combining the results</returns>
        /// <exception cref="StackOverflowException">If the current parser returns a zero-length successful result</exception>
        /// <seealso cref="Repeat{TSource, TResult}(Parser{TSource, TResult}, int)"/>
        /// <seealso cref="Many{TSource, TResult}(Parser{TSource, TResult}, int, int?)"/>
        /// <seealso cref="ManyUntil{TSource, TResult, TNext}(Parser{TSource, TResult}, Parser{TSource, TNext}, int)"/>
        /// <seealso cref="Skip{TSource, TResult}(Parser{TSource, TResult}, int)"/>
        /// <seealso cref="SkipMany{TSource, TResult}(Parser{TSource, TResult}, int, int?)"/>
        public static Parser<TSource, Tuple<int, TNext>> SkipUntil<TSource, TResult, TNext>(this Parser<TSource, TResult> self, Parser<TSource, TNext> next, int min = 0) {
            if (self == null || next == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(next));

            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min));

            return new Parser<TSource, Tuple<int, TNext>>((data, start, trace) => {
                var skipped = 0;
                var length = 0;

                while (true) {
                    var nextRes = next.Parse(data, start + length, trace);

                    if (nextRes.Success) {
                        return skipped < min
                            ? ParseResult.Fail<Tuple<int, TNext>>()
                            : ParseResult.Success(Tuple.Create(skipped, nextRes.Value), start, length + nextRes.Length);
                    }

                    var res = self.Parse(data, start + length, trace);

                    if (!res.Success)
                        return ParseResult.Fail<Tuple<int, TNext>>();

                    if (res.Length == 0)
                        ThrowOverflow(nameof(SkipUntil));

                    length += res.Length;
                    ++skipped;
                }
            });
        }

        /// <summary>
        /// Create a parser that returns a zero-length default result if the current parser fails
        /// </summary>
        /// <typeparam name="TSource">The type of items in the input data</typeparam>
        /// <typeparam name="TResult">The result type of <paramref name="self"/></typeparam>
        /// <param name="self">The current parser</param>
        /// <param name="defaultValue">The default value to return if the current parser fails</param>
        /// <returns>A new <see cref="Parser{TSource, TResult}"/> that executes the current parser, returning its result when successful or a default zero-length result otherwise</returns>
        public static Parser<TSource, TResult> Optional<TSource, TResult>(this Parser<TSource, TResult> self, TResult defaultValue = default) {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return new Parser<TSource, TResult>((data, start, trace) => {
                var res = self.Parse(data, start, trace);
                return res.Success ? res : ParseResult.Success(defaultValue, start, 0);
            }); 
        }

        private static void ThrowOverflow(string method) {
            throw new StackOverflowException($"call to {method}() with a zero-length parser detected");
        }
    }
}
