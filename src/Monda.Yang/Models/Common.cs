using System.Collections.Generic;

namespace Monda.Yang.Models {

    public interface INamed {
        string Name { get; set; }
    }

    public interface IStandardBlock {
        string Description { get; set; }
        string Reference { get; set; }
        IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
    }

    public interface IConditional {
        IList<string> Features { get; set; }
    }

    public interface ILifetime {
        string Status { get; set; }
    }

    public interface IDefinitionScope : INamed {
        IList<IType> Types { get; set; }
        IList<IGrouping> Groupings { get; set; }
    }
    
    public interface IParentScope : IDefinitionScope {
        IList<IProperty> Properties { get; set; }
        IList<string> Inherits { get; set; }
        IList<IChoice> Choices { get; set; }
    }    
}
