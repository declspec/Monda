using System.Collections.Generic;
using System.Text;

namespace Monda.Yang {
    public class YangStatement {
        public string Prefix { get; }
        public string Keyword { get; }
        public string Argument { get; }
        public IReadOnlyList<YangStatement> Children { get; }

        public YangStatement(string prefix, string keyword, string argument, IReadOnlyList<YangStatement> children) {
            Prefix = prefix;
            Keyword = keyword;
            Argument = argument;
            Children = children;
        }

        public override string ToString() {
            var builder = new StringBuilder();

            if (!string.IsNullOrEmpty(Prefix))
                builder.Append(Prefix + ":");

            builder.Append(Keyword);

            if (!string.IsNullOrEmpty(Argument))
                builder.Append(" " + Argument);

            if (Children?.Count > 0)
                builder.AppendFormat(" {{ ...{0} }}", Children.Count);
            else
                builder.Append(";");

            return builder.ToString();
        }
    }
}
