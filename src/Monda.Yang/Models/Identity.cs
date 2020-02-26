using System;
using System.Collections.Generic;
using System.Text;

namespace Monda.Yang.Models {

    public interface IIdentity : IStandardBlock, IConditional, INamed, ILifetime {
        IList<string> Bases { get; set; }
    }

    public class Identity : IIdentity {
        public IList<string> Bases { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public IList<string> Features { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Identity() { }

        public Identity(string name) {
            Name = name;
        }
    }
}
