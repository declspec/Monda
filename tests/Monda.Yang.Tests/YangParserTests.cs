using System;
using Xunit;

namespace Monda.Yang.Tests {
    public class YangParserTests {
        [Fact]
        public void ParseBasicYang() {
            var yang = "module test { }";
            var res = YangParser.KeywordParser.Then(YangParser.SeparatorParser).Parse(yang.AsSpan());
            var statements = YangParser.Parse(yang);
            Assert.Equal(1, statements.Count);
        }
    }
}
