using System;

namespace Monda {
    public class Parser<TSource, TResult> {
        public delegate ParseResult<TResult> ParseFunction(ReadOnlySpan<TSource> data, int start);

        public string Name { get; }

        private readonly ParseFunction _parse;

        public Parser(ParseFunction parse)
            : this(null, parse) { }

        public Parser(string name, ParseFunction parse) {
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
        
    }
}