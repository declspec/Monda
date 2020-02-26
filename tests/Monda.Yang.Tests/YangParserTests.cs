using System.IO;
using Xunit;

namespace Monda.Yang.Tests {
    public class YangParserTests {
        [Fact]
        public void ParseBasicYang() {
            var yang = "module test {}";
            var statements = YangParser.ParseStatements(yang);
            Assert.Equal(1, statements.Count);
        }

        [Fact]
        public void ParseComplicatedYang() {
            var yang = File.ReadAllText("./Examples/CISCO-PROCESS-MIB.yang");
            var module = YangParser.ParseModule(yang);
        }
    }
}
