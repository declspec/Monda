using System.Collections.Generic;

namespace Monda.Yang.Models {
    public interface IRestriction : IStandardBlock {
        string Expression { get; set; }
    }

    public interface IEnumValue : IStandardBlock, INamed, ILifetime {
        int Value { get; set; }
    }

    public interface IBitValue : IStandardBlock, INamed, ILifetime {
        int Position { get; set; }
    }

    public interface IType : IStandardBlock, INamed, ILifetime {
        string Units { get; set; }
        string BaseType { get; set; }
        string Default { get; set; }

        // Numeric fields:
        IRestriction Range { get; set; }
        int? FractionDigits { get; set; }

        // String fields:
        IList<IRestriction> Patterns { get; set; }

        // String / binary fields:
        IRestriction Length { get; set; }

        // Enumeration fields:
        IList<IEnumValue> Values { get; set; }

        // Bit fields:
        IList<IBitValue> Bits { get; set; }

        // Union fields: (TODO: Maybe just call it 'types'?)
        IList<IType> Unions { get; set; }

        // Leafref fields:
        string Path { get; set; } // XPath

        // IdentityRef fields:
        string Base { get; set; }

        // Instance-Identifier fields:
        bool? IsMandatory { get; set; }
    }

    public class BitValue : IBitValue {
        public int Position { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public BitValue() { }

        public BitValue(string name) {
            Name = name;
        }
    }

    public class EnumValue : IEnumValue {
        public int Value { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public EnumValue() { }

        public EnumValue(string name) {
            Name = name;
        }
    }

    public class Restriction : IRestriction {
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public string Expression { get; set; }

        public Restriction() { }

        public Restriction(string expression) {
            Expression = expression;
        }
    }

    public class YangType : IType {
        public string Units { get; set; }
        public string BaseType { get; set; }
        public string Default { get; set; }
        public IRestriction Range { get; set; }
        public int? FractionDigits { get; set; }
        public IList<IRestriction> Patterns { get; set; }
        public IRestriction Length { get; set; }
        public IList<IEnumValue> Values { get; set; }
        public IList<IBitValue> Bits { get; set; }
        public IList<IType> Unions { get; set; }
        public string Path { get; set; }
        public string Base { get; set; }
        public bool? IsMandatory { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public IList<KeyValuePair<string, string>> ExtensionValues { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public YangType() { }

        public YangType(string name) {
            Name = name;
        }
    }
}
