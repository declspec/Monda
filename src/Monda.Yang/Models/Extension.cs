using System.Collections.Generic;

namespace Monda.Yang.Models {
    public interface IExtensionArgument : IStandardBlock {
        bool? YinElement { get; set; }
    }

    public interface IExtension : IStandardBlock, INamed, ILifetime {
        IExtensionArgument Argument { get; set; }
    }

    public class ExtensionArgument : IExtensionArgument {
        public bool? YinElement { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
    }

    public class Extension : IExtension {
        public IExtensionArgument Argument { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Extension() { }

        public Extension(string name) {
            Name = name;
        }
    }
}
