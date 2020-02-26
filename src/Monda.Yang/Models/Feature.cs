using System.Collections.Generic;

namespace Monda.Yang.Models {
    public interface IFeature : IStandardBlock, IConditional, INamed, ILifetime {

    }

    public class Feature : IFeature {
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public IList<string> Features { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Feature() { }

        public Feature(string name) {
            Name = name;
        }
    }
}
