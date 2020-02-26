using System.Collections.Generic;

namespace Monda.Yang.Models {
    public interface IChoiceCase : IParentScope, IConditional, IStandardBlock, ILifetime {

    }

    public interface IChoice : IStandardBlock, IConditional, INamed, ILifetime {
        IList<IChoiceCase> Cases { get; set; }
        string Default { get; set; }
    }

    public class ChoiceCase : IChoiceCase {
        public IList<IProperty> Properties { get; set; }
        public IList<string> Inherits { get; set; }
        public IList<IChoice> Choices { get; set; }
        public IList<IType> Types { get; set; }
        public IList<IGrouping> Groupings { get; set; }
        public IList<string> Features { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public ChoiceCase() { }

        public ChoiceCase(string name) {
            Name = name;
        }
    }

    public class Choice : IChoice {
        public IList<IChoiceCase> Cases { get; set; }
        public string Default { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public IList<string> Features { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Choice() { }

        public Choice(string name) {
            Name = name;
        }
    }
}
