using System;
using System.Text;

namespace Monda.Yang {
    internal static class StringUtilities {
        public delegate void CreateStringAction<TState>(Span<char> span, TState state);

        public static string CreateString<TState>(int len, TState state, CreateStringAction<TState> action) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (len < 0)
                throw new ArgumentOutOfRangeException(nameof(len));

            var alloc = new string('\0', len);

            unsafe {
                var readable = alloc.AsSpan();

                fixed (char *ptr = &readable[0]) {
                    var writable = new Span<char>(ptr, readable.Length);
                    action(writable, state);
                }
            }

            return alloc;
        }

        public static StringBuilder Append(StringBuilder sb, in ReadOnlySpan<char> data) {
            if (data.IsEmpty)
                return sb;

            unsafe {
                fixed (char* ptr = &data[0])
                    return sb.Append(ptr, data.Length);
            }
        }
    }
}
