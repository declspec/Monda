using System.Collections.Generic;

namespace Monda.Yang.Models {
    public interface IProperty : IStandardBlock, IConditional, INamed, ILifetime {
        bool? IsConfig { get; set; }

        bool IsEnumerable { get; set; }
        int? MinLength { get; set; }
        int? MaxLength { get; set; }
        string OrderedBy { get; set; }
    }

    public interface IContainer : IProperty, IParentScope {
        string Presence { get; set; }
    }

    public interface ILeaf : IProperty {
        IType Type { get; set; }
        string Default { get; set; }
        string Units { get; set; }
    }

    public abstract class Property : IProperty {
        public bool? IsConfig { get; set; }
        public bool IsEnumerable { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public string OrderedBy { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public IList<string> Features { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public class Container : Property, IContainer {
        public string Presence { get; set; }
        public IList<IProperty> Properties { get; set; }
        public IList<string> Inherits { get; set; }
        public IList<IChoice> Choices { get; set; }
        public IList<IType> Types { get; set; }
        public IList<IGrouping> Groupings { get; set; }

        public Container() { }

        public Container(string name) {
            Name = name;
        }
    }

    public class Leaf : Property, ILeaf {
        public IType Type { get; set; }
        public string Default { get; set; }
        public string Units { get; set; }

        public Leaf() { }
        public Leaf(string name) {
            Name = name;
        }
    }
}
