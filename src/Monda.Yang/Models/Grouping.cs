using System.Collections.Generic;

namespace Monda.Yang.Models {
    public interface IGrouping : IParentScope, IStandardBlock, ILifetime {

    }

    public class Grouping : IGrouping {
        public IList<IProperty> Properties { get; set; }
        public IList<string> Inherits { get; set; }
        public IList<IChoice> Choices { get; set; }
        public IList<IType> Types { get; set; }
        public IList<IGrouping> Groupings { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Grouping() { }

        public Grouping(string name) {
            Name = name;
        }
    }
}
