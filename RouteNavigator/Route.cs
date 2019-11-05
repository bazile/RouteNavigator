using System;
using System.Text.RegularExpressions;

namespace RouteNavigator
{
    public class Route //: IComparable<Route>
    {
        public string Prefix { get; }
        public string Template { get; }
        public string ClassName { get; }
        public string MethodName { get; }
        public string HttpMethod { get; }
        public string FullRoute { get; }
        public string NormalizedFullRoute { get; }

        public Route(string prefix, string template, string className, string methodName, string httpMethod)
        {
            Prefix = prefix;
            Template = template;
            ClassName = className;
            MethodName = methodName;
            HttpMethod = httpMethod;

            FullRoute = (prefix + "/" + template).TrimStart('/');
            NormalizedFullRoute = Normalize(FullRoute);
        }

        public static string Normalize(string template)
        {
            const string EXPR = @"
(
    \{[^\}]+\}
    |
    [0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}
)";
            return Regex.Replace(template, EXPR, "{xyz}", RegexOptions.IgnorePatternWhitespace);
        }

        //public int CompareTo(Route other)
        //{
        //    if (ReferenceEquals(this, other)) return 0;
        //    if (ReferenceEquals(null, other)) return 1;
        //    int prefixComparison = string.Compare(Prefix, other.Prefix, StringComparison.Ordinal);
        //    if (prefixComparison != 0) return prefixComparison;
        //    return string.Compare(Template, other.Template, StringComparison.Ordinal);
        //}
    }
}