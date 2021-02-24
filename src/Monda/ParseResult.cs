using System;

namespace Monda {
    /// <summary>
    /// Represents the result of a parsing operation
    /// </summary>
    /// <typeparam name="TValue">Underlying value type</typeparam>
    /// <seealso cref="ParseResult.Success{T}(in T, int, int)"/>
    /// <seealso cref="ParseResult.Fail{T}()"/>
    /// <seealso cref="ParseResult.Fail{T}(in T)"/>
    public readonly ref struct ParseResult<TValue> {
        /// <summary>The underlying result value</summary>
        public TValue Value { get; }
        /// <summary>The position in the data source where this result started</summary>
        public int Start { get; }
        /// <summary>The length of data in the data source used to construct this result</summary>
        public int Length { get; }
        /// <summary>Indicates if this result was successful</summary>
        public bool Success => Start >= 0 && Length >= 0;

        public ParseResult(in TValue value, int start, int length) {
            Value = value;
            Start = start;
            Length = length;
        }
    }

    /// <summary>
    /// Generic helper functions for constructing <see cref="ParseResult{TValue}"/> instances
    /// </summary>
    public static class ParseResult {
        /// <summary>
        /// Create a failure result with the default value for <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The underlying value type</typeparam>
        /// <returns>A failure <see cref="ParseResult{TValue}"/> with it's value set to default(<typeparamref name="T"/>)</returns>
        public static ParseResult<T> Fail<T>() => Fail(default(T));

        /// <summary>
        /// Create a failure result with a specified value
        /// </summary>
        /// <typeparam name="T">The underlying value type</typeparam>
        /// <param name="value">The value of the result</param>
        /// <returns>A failure <see cref="ParseResult{TValue}"/> with it's value set to <paramref name="value"/></returns>
        public static ParseResult<T> Fail<T>(in T value) => new ParseResult<T>(value, -1, -1);

        /// <summary>
        /// Create a success result
        /// </summary>
        /// <typeparam name="T">The underlying value type</typeparam>
        /// <param name="value">The value of the result</param>
        /// <param name="start">The position at which the result started</param>
        /// <param name="length">The length of data used to parse <paramref name="value"/></param>
        /// <returns></returns>
        public static ParseResult<T> Success<T>(in T value, int start, int length) {
            return (length >= 0 && start >= 0)
                ? new ParseResult<T>(value, start, length)
                : throw new ArgumentOutOfRangeException(length < 0 ? nameof(length) : nameof(start));
        }
    }
}
