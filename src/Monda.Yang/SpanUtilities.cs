using System;
using System.Collections.Generic;

namespace Monda.Yang {
    internal static class SpanUtilities {
        public static bool Contains<T>(in ReadOnlySpan<T> span, T value, IEqualityComparer<T> comparer) {
            for (var i = 0; i < span.Length; ++i) {
                if (comparer.Equals(span[i], value))
                    return true;
            }

            return false;
        }
    }
}