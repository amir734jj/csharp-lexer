using System.Text.RegularExpressions;

namespace Lexer.core.Extensions
{
    internal static class RegexExtension
    {
        public static bool FullMatch(this Regex regex, string token)
        {
            var result = Regex.Match(token, regex.ToString());

            return result.Success && result.Value.Equals(token);
        }
    }
}