using System.Collections.Generic;

namespace Monda.Yang.Models {
    public interface IRpcParameter : IStandardBlock, IParentScope {

    }

    public interface IRpc : IStandardBlock, IDefinitionScope, IConditional, ILifetime {
        IRpcParameter Input { get; set; }
        IRpcParameter Output { get; set; }
    }

    public class RpcParameter : IRpcParameter {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public IList<IProperty> Properties { get; set; }
        public IList<string> Inherits { get; set; }
        public IList<IChoice> Choices { get; set; }
        public IList<IType> Types { get; set; }
        public IList<IGrouping> Groupings { get; set; }

        public RpcParameter(string name) {
            Name = name;
        }
    }

    public class Rpc : IRpc {
        public IRpcParameter Input { get; set; }
        public IRpcParameter Output { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public IList<IType> Types { get; set; }
        public IList<IGrouping> Groupings { get; set; }
        public IList<string> Features { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Rpc() { }

        public Rpc(string name) {
            Name = name;
        }
    }
}
