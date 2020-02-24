using System.IO;
using Xunit;

namespace Monda.Yang.Tests {
    public class YangParserTests {
        [Fact]
        public void ParseBasicYang() {
            var yang = "module test {}";
            var statements = YangParser.Parse(yang);
            Assert.Equal(1, statements.Count);
        }

        [Fact]
        public void ParseComplicatedYang() {
            var yang = File.ReadAllText("./Examples/CISCO-PROCESS-MIB.yang");
            var statements = YangParser.Parse(yang);
            Assert.Equal(1, statements.Count);
            Assert.True(statements[0].Children.Count > 0);
        }
    }
}
