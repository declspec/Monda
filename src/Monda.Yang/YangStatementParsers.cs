using System;
using System.Collections.Generic;
using Monda.Yang.Models;

namespace Monda.Yang {
    internal static class YangStatementParsers {
        private delegate bool StatementHandler<T>(YangStatement statement, T partial);

        private static readonly StatementHandler<IFeature>[] FeatureHandlers = { ConditionalHandler, StandardBlockHandler };
        private static readonly StatementHandler<IRestriction>[] RestrictionHandlers = { StandardBlockHandler };
        private static readonly StatementHandler<IType>[] TypeHandlers = { TypeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IType>[] TypedefHandlers = { TypedefHandler, LifetimeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IEnumValue>[] EnumValueHandlers = { EnumValueHandler, LifetimeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IBitValue>[] BitValueHandlers = { BitValueHandler, LifetimeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IExtensionArgument>[] ExtensionArgumentHandlers = { ExtensionArgumentHandler };
        private static readonly StatementHandler<IExtension>[] ExtensionHandlers = { ExtensionHandler, LifetimeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IIdentity>[] IdentityHandlers = { IdentityHandler, ConditionalHandler, LifetimeHandler, StandardBlockHandler };
        private static readonly StatementHandler<ILeaf>[] LeafHandlers = { LeafHandler, PropertyHandler, ConditionalHandler, LifetimeHandler, StandardBlockHandler };
        private static readonly StatementHandler<ILeaf>[] AnyDataHandlers = { PropertyHandler, ConditionalHandler, LifetimeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IContainer>[] ContainerHandlers = { ContainerHandler, DefinitionScopeHandler, ParentScopeHandler, PropertyHandler, ConditionalHandler, LifetimeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IGrouping>[] GroupingHandlers = { DefinitionScopeHandler, ParentScopeHandler, LifetimeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IImport>[] ImportHandlers = { ImportHandler, StandardBlockHandler };
        private static readonly StatementHandler<IRevision>[] RevisionHandlers = { StandardBlockHandler };
        private static readonly StatementHandler<IYangModule>[] ModuleHandlers = { ModuleHandler, DefinitionScopeHandler, ParentScopeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IRpcParameter>[] RpcParameterHandlers = { DefinitionScopeHandler, ParentScopeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IRpc>[] RpcHandlers = { RpcHandler, DefinitionScopeHandler, LifetimeHandler, StandardBlockHandler };
        private static readonly StatementHandler<IChoiceCase>[] ChoiceCaseHandlers = { DefinitionScopeHandler, ParentScopeHandler, LifetimeHandler, ConditionalHandler, StandardBlockHandler };
        private static readonly StatementHandler<IChoice>[] ChoiceHandlers = { ChoiceHandler, LifetimeHandler, ConditionalHandler, StandardBlockHandler };
        private static readonly StatementHandler<IBelongsTo>[] BelongsToHandlers = { BelongsToHandler };

        public static IYangModule ParseModule(YangStatement statement) {
            if (statement == null)
                throw new ArgumentNullException(nameof(statement));

            return ParseStatement(statement, ModuleHandlers, new YangModule(statement.Argument));
        }

        private static T ParseStatement<T>(YangStatement statement, IList<StatementHandler<T>> handlers, T partial) {
            if (statement.Children != null) {
                foreach (var child in statement.Children) {
                    foreach (var handler in handlers) {
                        if (handler(child, partial))
                            break;
                    }
                }
            }

            return partial;
        }

        private static IType ParseType(YangStatement statement, IType partial = default) {
            return ParseStatement(statement, TypeHandlers, partial ?? new YangType() {
                Name = statement.Argument
            });
        }

        private static bool TypedefHandler(YangStatement statement, IType partial) {
            switch (statement.Keyword) {
                case "default":
                    partial.Default = statement.Argument;
                    return true;
                case "units":
                    partial.Units = statement.Argument;
                    return true;
                case "type":
                    ParseType(statement, partial);
                    partial.BaseType = statement.Argument;
                    return true;
                default:
                    return false;
            }
        }

        private static bool TypeHandler(YangStatement statement, IType partial) {
            switch (statement.Keyword) {
                case "base":
                    partial.Base = statement.Argument;
                    return true;
                case "path":
                    partial.Path = statement.Argument;
                    return true;
                case "pattern":
                    if (partial.Patterns == null)
                        partial.Patterns = new List<IRestriction>();

                    partial.Patterns.Add(ParseStatement(statement, RestrictionHandlers, new Restriction(statement.Argument)));
                    return true;
                case "length":
                    partial.Length = ParseStatement(statement, RestrictionHandlers, new Restriction(statement.Argument));
                    return true;
                case "range":
                    partial.Range = ParseStatement(statement, RestrictionHandlers, new Restriction(statement.Argument));
                    return true;
                case "require-instance":
                    partial.IsMandatory = statement.Argument != "false";
                    return true;
                case "type":
                    if (partial.Unions == null)
                        partial.Unions = new List<IType>();

                    partial.Unions.Add(ParseType(statement));
                    return true;
                case "bit":
                    if (partial.Bits == null)
                        partial.Bits = new List<IBitValue>();

                    partial.Bits.Add(ParseStatement(statement, BitValueHandlers, new BitValue(statement.Argument)));
                    return true;
                case "enum":
                    if (partial.Values == null)
                        partial.Values = new List<IEnumValue>();

                    partial.Values.Add(ParseStatement(statement, EnumValueHandlers, new EnumValue(statement.Argument)));
                    return true;
                default:
                    return false;
            }
        }

        private static bool EnumValueHandler(YangStatement statement, IEnumValue partial) {
            if (statement.Keyword != "value")
                return false;

            partial.Value = int.Parse(statement.Argument);
            return true;
        }

        private static bool BitValueHandler(YangStatement statement, IBitValue partial) {
            if (statement.Keyword != "position")
                return false;

            partial.Position = int.Parse(statement.Argument);
            return true;
        }

        private static bool ExtensionArgumentHandler(YangStatement statement, IExtensionArgument partial) {
            if (statement.Keyword != "yin-element" || statement.Argument == null)
                return false;

            partial.YinElement = statement.Argument != "false";
            return true;
        }

        private static bool ExtensionHandler(YangStatement statement, IExtension partial) {
            if (statement.Keyword != "argument")
                return false;

            partial.Argument = ParseStatement(statement, ExtensionArgumentHandlers, new ExtensionArgument());
            return true;
        }

        private static bool StandardBlockHandler(YangStatement statement, IStandardBlock partial) {
            switch (statement.Keyword) {
                case "description":
                    partial.Description = statement.Argument;
                    return true;
                case "reference":
                    partial.Reference = statement.Argument;
                    return true;
            }

            if (statement.Keyword.IndexOf(":") < 0)
                return false;

            if (partial.ExtensionValues == null)
                partial.ExtensionValues = new List<KeyValuePair<string, string>>();

            partial.ExtensionValues.Add(new KeyValuePair<string, string>(statement.Keyword, statement.Argument));
            return true;
        }

        private static bool IdentityHandler(YangStatement statement, IIdentity partial) {
            if (statement.Keyword != "base")
                return false;

            if (partial.Bases == null)
                partial.Bases = new List<string>();

            partial.Bases.Add(statement.Argument);
            return true;
        }

        private static bool LifetimeHandler(YangStatement statement, ILifetime partial) {
            if (statement.Keyword != "status")
                return false;

            partial.Status = statement.Argument;
            return true;
        }

        private static bool ConditionalHandler(YangStatement statement, IConditional partial) {
            if (statement.Keyword != "if-feature")
                return false;

            if (partial.Features == null)
                partial.Features = new List<string>();

            partial.Features.Add(statement.Argument);
            return true;
        }

        private static bool PropertyHandler(YangStatement statement, IProperty partial) {
            switch (statement.Keyword) {
                case "config":
                    partial.IsConfig = statement.Argument == "true";
                    return true;
                case "max-elements":
                    partial.MaxLength = int.Parse(statement.Argument);
                    return true;
                case "min-elements":
                    partial.MinLength = int.Parse(statement.Argument);
                    return true;
                case "ordered-by":
                    partial.OrderedBy = statement.Argument;
                    return true;
                default:
                    return false;
            }
        }

        private static bool LeafHandler(YangStatement statement, ILeaf partial) {
            switch (statement.Keyword) {
                case "type":
                    partial.Type = ParseType(statement);
                    return true;
                case "default":
                    partial.Default = statement.Argument;
                    return true;
                case "units":
                    partial.Units = statement.Argument;
                    return true;
                default:
                    return false;
            }
        }

        private static bool ContainerHandler(YangStatement statement, IContainer partial) {
            if (statement.Keyword != "presence")
                return false;

            partial.Presence = statement.Argument;
            return true;
        }

        private static bool RpcHandler(YangStatement statement, IRpc partial) {
            switch (statement.Keyword) {
                case "input":
                    partial.Input = ParseStatement(statement, RpcParameterHandlers, new RpcParameter(statement.Keyword));
                    return false;
                case "output":
                    partial.Output = ParseStatement(statement, RpcParameterHandlers, new RpcParameter(statement.Keyword));
                    return false;
                default:
                    return false;
            }
        }

        private static bool ImportHandler(YangStatement statement, IImport partial) {
            switch (statement.Keyword) {
                case "prefix":
                    partial.Prefix = statement.Argument;
                    return true;
                case "revision-date":
                    partial.Revision = statement.Argument;
                    return true;
                default:
                    return false;
            }
        }

        private static bool BelongsToHandler(YangStatement statement, IBelongsTo partial) {
            switch (statement.Keyword) {
                case "prefix":
                    partial.Prefix = statement.Argument;
                    return true;
                default:
                    return false;
            }
        }

        private static bool ChoiceHandler(YangStatement statement, IChoice partial) {
            switch (statement.Keyword) {
                case "leaf":
                case "leaf-list":
                case "container":
                case "list":
                case "anydata":
                case "anyxml":
                case "case":
                    if (partial.Cases == null)
                        partial.Cases = new List<IChoiceCase>();

                    var choiceCase = new ChoiceCase(statement.Argument);

                    if (statement.Keyword == "case")
                        ParseStatement(statement, ChoiceCaseHandlers, choiceCase);
                    else {
                        // Any top level props automagically map to a case with the same name
                        ParentScopeHandler(statement, choiceCase);
                        DefinitionScopeHandler(statement, choiceCase);
                    }

                    partial.Cases.Add(choiceCase);
                    return true;
                case "default":
                    partial.Default = statement.Argument;
                    return true;
                default:
                    return false;
            }
        }

        private static bool DefinitionScopeHandler(YangStatement statement, IDefinitionScope partial) {
            switch (statement.Keyword) {
                case "typedef":
                    if (partial.Types == null)
                        partial.Types = new List<IType>();

                    partial.Types.Add(ParseStatement(statement, TypedefHandlers, new YangType(statement.Argument)));
                    return true;
                case "grouping":
                    if (partial.Groupings == null)
                        partial.Groupings = new List<IGrouping>();

                    partial.Groupings.Add(ParseStatement(statement, GroupingHandlers, new Grouping(statement.Argument)));
                    return true;
                default:
                    return false;
            }
        }

        private static bool ParentScopeHandler(YangStatement statement, IParentScope partial) {
            switch (statement.Keyword) {
                case "uses":
                    if (partial.Inherits == null)
                        partial.Inherits = new List<string>();

                    partial.Inherits.Add(statement.Argument);
                    return true;
                case "anyxml":
                case "anydata":
                    if (partial.Properties == null)
                        partial.Properties = new List<IProperty>();

                    partial.Properties.Add(ParseStatement(statement, AnyDataHandlers, new Leaf(statement.Argument) {
                        Type = new YangType(statement.Keyword)
                    }));

                    return true;
                case "leaf":
                case "leaf-list":
                    if (partial.Properties == null)
                        partial.Properties = new List<IProperty>();

                    partial.Properties.Add(ParseStatement(statement, LeafHandlers, new Leaf(statement.Argument) {
                        IsEnumerable = statement.Keyword == "leaf-list"
                    }));

                    return true;
                case "container":
                case "list":
                    if (partial.Properties == null)
                        partial.Properties = new List<IProperty>();

                    partial.Properties.Add(ParseStatement(statement, ContainerHandlers, new Container(statement.Argument) {
                        IsEnumerable = statement.Keyword == "list"
                    }));

                    return true;
                case "choice":
                    if (partial.Choices == null)
                        partial.Choices = new List<IChoice>();

                    partial.Choices.Add(ParseStatement(statement, ChoiceHandlers, new Choice(statement.Argument)));
                    return true;
                default:
                    return false;
            }
        }

        private static bool ModuleHandler(YangStatement statement, IYangModule partial) {
            switch (statement.Keyword) {
                case "belongs-to":
                    partial.BelongsTo = ParseStatement(statement, BelongsToHandlers, new BelongsTo(statement.Argument));
                    return true;
                case "prefix":
                    partial.Prefix = statement.Argument;
                    return true;
                case "namespace":
                    partial.Namespace = statement.Argument;
                    return true;
                case "organization":
                    partial.Organisation = statement.Argument;
                    return true;
                case "contact":
                    partial.Contact = statement.Argument;
                    return true;
                case "yang-version":
                    partial.YangVersion = statement.Argument;
                    return true;
                case "feature":
                    if (partial.Features == null)
                        partial.Features = new List<IFeature>();

                    partial.Features.Add(ParseStatement(statement, FeatureHandlers, new Feature(statement.Argument)));
                    return true;
                case "identity":
                    if (partial.Identities == null)
                        partial.Identities = new List<IIdentity>();

                    partial.Identities.Add(ParseStatement(statement, IdentityHandlers, new Identity(statement.Argument)));
                    return true;
                case "import":
                    if (partial.Imports == null)
                        partial.Imports = new List<IImport>();

                    partial.Imports.Add(ParseStatement(statement, ImportHandlers, new Import(statement.Argument)));
                    return true;
                case "include":
                    if (partial.Includes == null)
                        partial.Includes = new List<string>();

                    partial.Includes.Add(statement.Argument);
                    return true;
                case "extension":
                    if (partial.Extensions == null)
                        partial.Extensions = new List<IExtension>();

                    partial.Extensions.Add(ParseStatement(statement, ExtensionHandlers, new Extension(statement.Argument)));
                    return true;
                case "revision":
                    if (partial.Revisions == null)
                        partial.Revisions = new List<IRevision>();

                    partial.Revisions.Add(ParseStatement(statement, RevisionHandlers, new Revision(statement.Argument)));
                    return true;
                case "rpc":
                    if (partial.Rpcs == null)
                        partial.Rpcs = new List<IRpc>();

                    partial.Rpcs.Add(ParseStatement(statement, RpcHandlers, new Rpc(statement.Argument)));
                    return true;
                default:
                    return false;
            }
        }
    }
}
