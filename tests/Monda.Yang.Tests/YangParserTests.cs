using System.IO;
using Xunit;

namespace Monda.Yang.Tests {
    public class YangParserTests {
        [Fact]
        public void ParseBasicYang() {
            var yang = "submodule etsi-nfv-vnf {}";
            var statements = YangParser.ParseStatements(yang);
            Assert.Equal(1, statements.Count);
        }

        [Theory]
        [InlineData("etsi-nfv-vnf")]
        [InlineData("CISCO-PROCESS-MIB")]
        public void ParseComplicatedYang(string filename) {
            var yang = File.ReadAllText(Path.Join("./Examples", $"{filename}.yang"));
            var module = YangParser.ParseModule(yang);
        }
    }
}
