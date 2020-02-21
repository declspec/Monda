using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Monda.Csv.Tests {
    public class CsvParserTests {
        [Fact]
        public void ParsesSimpleCsv() {
            var expected = new[] { "one", "two", "three" };
            var csv = string.Join(',', expected);

            var result = CsvParser.Parse(csv);

            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0]);
        }

        [Fact]
        public void ParsesMultilineCsv() {
            TestCsv(new[] {
                new [] { "one", "two", "three" },
                new [] { "four", "five", "six" },
                new [] { "seven", "eight", "nine", "ten" }
            });
        }

        [Fact]
        public void ParsesQuotedStrings() {
            TestCsv(new[] {
                new [] { "one", "two,three with\nmultiple\nlines", "four with spaces" },
                new [] { "seven", "eight\"\"\"\nnine" }
            });
        }

        private static void TestCsv(IReadOnlyList<IReadOnlyList<string>> records) {
            var csv = string.Join('\n', records.Select(r => string.Join(',', r.Select(EscapeString))));
            var result = CsvParser.Parse(csv);

            Assert.Equal(records.Count, result.Count);

            for (var i = 0; i < records.Count; ++i) {
                Assert.Equal(records[i], result[i]);
            }
        }

        private static string EscapeString(string str) {
            return str.IndexOfAny("\",\n".ToCharArray()) >= 0
                ? $"\"{str.Replace("\"", "\"\"")}\""
                : str;
        }
    }
}
