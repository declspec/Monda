using System;
using System.Collections.Generic;

namespace Monda {
    public static class CommonExtensions {
        public static Parser<TSource, Tuple<TResult, TNext>> Then<TSource, TResult, TNext>(this Parser<TSource, TResult> self, Parser<TSource, TNext> next) {
            if (self == null || next == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(next));

            return new Parser<TSource, Tuple<TResult, TNext>>((data, start) => {
                var selfRes = self.Parse(data, start);

                if (!selfRes.Success)
                    return ParseResult.Fail<Tuple<TResult, TNext>>();

                var nextRes = next.Parse(data, start + selfRes.Length);

                return nextRes.Success
                    ? ParseResult.Success(Tuple.Create(selfRes.Value, nextRes.Value), start, selfRes.Length + nextRes.Length)
                    : ParseResult.Fail<Tuple<TResult, TNext>>();
            });
        }

        public static Parser<TSource, TResult> PrecededBy<TSource, TResult, TPrevious>(this Parser<TSource, TResult> self, Parser<TSource, TPrevious> previous) {
            if (self == null || previous == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(previous));

            return new Parser<TSource, TResult>((data, start) => {
                var previousRes = previous.Parse(data, start);

                if (!previousRes.Success)
                    return ParseResult.Fail<TResult>();

                var selfRes = self.Parse(data, start + previousRes.Length);

                return selfRes.Success
                    ? ParseResult.Success(selfRes.Value, start, previousRes.Length + selfRes.Length)
                    : ParseResult.Fail<TResult>();
            });
        }

        public static Parser<TSource, TResult> FollowedBy<TSource, TResult, TNext>(this Parser<TSource, TResult> self, Parser<TSource, TNext> next) {
            if (self == null || next == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(next));

            return new Parser<TSource, TResult>((data, start) => {
                var selfRes = self.Parse(data, start);

                if (!selfRes.Success)
                    return selfRes;

                var nextRes = next.Parse(data, start + selfRes.Length);

                return nextRes.Success 
                    ? ParseResult.Success(selfRes.Value, start, selfRes.Length + nextRes.Length) 
                    : ParseResult.Fail<TResult>();
            });
        }

        public static Parser<TSource, TResult> Or<TSource, TResult>(this Parser<TSource, TResult> self, Parser<TSource, TResult> other) {
            if (self == null || other == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(other));

            return new Parser<TSource, TResult>((data, start) => {
                var res = self.Parse(data, start);
                return res.Success ? res : other.Parse(data, start);
            });
        }

        public static Parser<TSource, TResult> Between<TSource, TResult, TLeft, TRight>(this Parser<TSource, TResult> self, Parser<TSource, TLeft> left, Parser<TSource, TRight> right) {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            if (left == null || right == null)
                throw new ArgumentNullException(left == null ? nameof(left) : nameof(right));

            return new Parser<TSource, TResult>((data, start) => {
                var leftRes = left.Parse(data, start);
                var length = 0;

                if (!leftRes.Success)
                    return ParseResult.Fail<TResult>();

                length += leftRes.Length;
                var selfRes = self.Parse(data, start + length);

                if (!selfRes.Success)
                    return ParseResult.Fail<TResult>();

                length += selfRes.Length;
                var rightRes = right.Parse(data, start + length);

                if (!rightRes.Success)
                    return ParseResult.Fail<TResult>();

                length += rightRes.Length;
                return ParseResult.Success(selfRes.Value, start, length);
            });
        }

        public static Parser<TSource, TNext> Map<TSource, TResult, TNext>(this Parser<TSource, TResult> self, Func<TResult, TNext> map) {
            if (self == null || map == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(map));

            return new Parser<TSource, TNext>((data, start) => {
                var res = self.Parse(data, start);
                return res.Success ? ParseResult.Success(map(res.Value), start, res.Length) : ParseResult.Fail<TNext>();
            });
        }

        public static Parser<TSource, IReadOnlyList<TResult>> Repeat<TSource, TResult>(this Parser<TSource, TResult> self, int count) {
            return Many(self, count, count);
        }

        public static Parser<TSource, IReadOnlyList<TResult>> Many<TSource, TResult>(this Parser<TSource, TResult> self, int min = 0, int? max = default) {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min));

            if (max.HasValue && max < min)
                throw new ArgumentOutOfRangeException(nameof(max));

            return new Parser<TSource, IReadOnlyList<TResult>>((data, start) => {
                var values = new List<TResult>();
                var length = 0;

                while (max == default || values.Count < max) {
                    var res = self.Parse(data, start + length);

                    if (!res.Success)
                        break;

                    // TODO: Maybe an exception if max == default && res.Length == 0?
                    //  if the parser is deterministic then this will infinitely loop.

                    values.Add(res.Value);
                    length += res.Length;
                }

                return values.Count >= min
                    ? ParseResult.Success((IReadOnlyList<TResult>)values, start, length)
                    : ParseResult.Fail<IReadOnlyList<TResult>>();
            });
        }

        public static Parser<TSource, Tuple<IReadOnlyList<TResult>, TNext>> ManyUntil<TSource, TResult, TNext>(this Parser<TSource, TResult> self, Parser<TSource, TNext> next, int min = 0) {
            if (self == null || next == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(next));

            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min));

            return new Parser<TSource, Tuple<IReadOnlyList<TResult>, TNext>>((data, start) => {
                var values = new List<TResult>();
                var length = 0;

                while (true) {
                    var nextRes = next.Parse(data, start + length);

                    if (nextRes.Success) {
                        return values.Count < min
                            ? ParseResult.Fail<Tuple<IReadOnlyList<TResult>, TNext>>()
                            : ParseResult.Success(new Tuple<IReadOnlyList<TResult>, TNext>(values, nextRes.Value), start, length + nextRes.Length);
                    }
                    
                    var res = self.Parse(data, start + length);

                    if (!res.Success)
                        return ParseResult.Fail<Tuple<IReadOnlyList<TResult>, TNext>>();

                    // TODO: Maybe an exception if res.Length == 0?
                    //  if the parsers are deterministic then this will infinitely loop.
                    length += res.Length;
                    values.Add(res.Value);
                }
            });
        }

        public static Parser<TSource, int> Skip<TSource, TResult>(this Parser<TSource, TResult> self, int count) {
            return SkipMany(self, count, count);
        }

        public static Parser<TSource, int> SkipMany<TSource, TResult>(this Parser<TSource, TResult> self, int min = 0, int? max = default) {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min));

            if (max.HasValue && max < min)
                throw new ArgumentOutOfRangeException(nameof(max));

            return new Parser<TSource, int>((data, start) => {
                var skipped = 0;
                var length = 0;

                while (max == default || skipped < max) {
                    var res = self.Parse(data, start + length);

                    if (!res.Success)
                        break;

                    // TODO: Maybe an exception if max == default && res.Length == 0?
                    //  if the parser is deterministic then this will infinitely loop.
                    length += res.Length;
                    ++skipped;   
                }

                return skipped >= min
                    ? ParseResult.Success(skipped, start, length)
                    : ParseResult.Fail<int>();
            });
        }

        public static Parser<TSource, Tuple<int, TNext>> SkipUntil<TSource, TResult, TNext>(this Parser<TSource, TResult> self, Parser<TSource, TNext> next, int min = 0) {
            if (self == null || next == null)
                throw new ArgumentNullException(self == null ? nameof(self) : nameof(next));

            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min));

            return new Parser<TSource, Tuple<int, TNext>>((data, start) => {
                var skipped = 0;
                var length = 0;

                while (true) {
                    var nextRes = next.Parse(data, start + length);

                    if (nextRes.Success) {
                        return skipped < min
                            ? ParseResult.Fail<Tuple<int, TNext>>()
                            : ParseResult.Success(Tuple.Create(skipped, nextRes.Value), start, length + nextRes.Length);
                    }

                    var res = self.Parse(data, start + length);

                    if (!res.Success)
                        return ParseResult.Fail<Tuple<int, TNext>>();

                    // TODO: Maybe an exception if res.Length == 0?
                    //  if the parsers are deterministic then this will infinitely loop.
                    length += res.Length;
                    ++skipped;
                }
            });
        }

        public static Parser<TSource, TResult> Optional<TSource, TResult>(this Parser<TSource, TResult> self, TResult defaultValue = default) {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return new Parser<TSource, TResult>((data, start) => {
                var res = self.Parse(data, start);
                return res.Success ? res : ParseResult.Success(defaultValue, start, 0);
            }); 
        }
    }
}
