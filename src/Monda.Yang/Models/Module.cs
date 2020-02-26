using System.Collections.Generic;

namespace Monda.Yang.Models {
    public interface IRevision : IStandardBlock {
        string Date { get; set; }
    }

    public interface IImport : IStandardBlock, INamed {
        string Prefix { get; set; }
        string Revision { get; set; }
    }

    public interface IBelongsTo : INamed {
        string Prefix { get; set; }
    }

    public interface IYangModule : IParentScope, IStandardBlock {
        string Namespace { get; set; }
        string Prefix { get; set; }
        string YangVersion { get; set; }

        string Contact { get; set; }
        string Organisation { get; set; }
        IBelongsTo BelongsTo { get; set; }
        IList<IIdentity> Identities { get; set; }
        IList<IExtension> Extensions { get; set; }
        IList<IFeature> Features { get; set; }
        IList<IImport> Imports { get; set; }
        IList<IRevision> Revisions { get; set; }
        IList<string> Includes { get; set; }
        IList<IRpc> Rpcs { get; set; }
    }

    public class Revision : IRevision {
        public string Date { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }

        public Revision() { }

        public Revision(string date) {
            Date = date;
        }
    }

    public class Import : IImport {
        public string Prefix { get; set; }
        public string Revision { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public string Name { get; set; }

        public Import() { }

        public Import(string name) {
            Name = name;
        }
    }

    public class BelongsTo : IBelongsTo {
        public string Prefix { get; set; }
        public string Name { get; set; }

        public BelongsTo(string name) {
            Name = name;
        }
    }

    public class YangModule : IYangModule {
        public string Namespace { get; set; }
        public string Prefix { get; set; }
        public string YangVersion { get; set; }
        public string Contact { get; set; }
        public string Organisation { get; set; }
        public IBelongsTo BelongsTo { get; set; }
        public IList<IIdentity> Identities { get; set; }
        public IList<IExtension> Extensions { get; set; }
        public IList<IFeature> Features { get; set; }
        public IList<IImport> Imports { get; set; }
        public IList<IRevision> Revisions { get; set; }
        public IList<string> Includes { get; set; }
        public IList<IRpc> Rpcs { get; set; }
        public IList<IProperty> Properties { get; set; }
        public IList<string> Inherits { get; set; }
        public IList<IChoice> Choices { get; set; }
        public IList<IType> Types { get; set; }
        public IList<IGrouping> Groupings { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public string Name { get; set; }

        public YangModule() { }

        public YangModule(string name) {
            Name = name;
        }
    }
}
