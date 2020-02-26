using System;

namespace Monda {
    public readonly ref struct ParseResult<TValue> {
        public readonly TValue Value;
        public readonly int Start;
        public readonly int Length;
        public bool Success => Start >= 0;

        public ParseResult(in TValue value, int start, int length) {
            Value = value;
            Start = start;
            Length = length;
        }
    }

    public static class ParseResult {
        public static ParseResult<T> Fail<T>() => Fail(default(T));
        public static ParseResult<T> Fail<T>(in T value) => new ParseResult<T>(value, -1, -1);

        public static ParseResult<T> Success<T>(in T value, int start, int length) {
            return (length >= 0 && start >= 0)
                ? new ParseResult<T>(value, start, length)
                : throw new ArgumentOutOfRangeException(length < 0 ? nameof(length) : nameof(start));
        }
    }
}
