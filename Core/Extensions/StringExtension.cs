using System.IO;
using System.Text;

namespace Lexer.core.Extensions
{
    internal static class StringExtension
    {
        public static string TakeFirst(this string str, string token)
        {
            return str.Length > token.Length ? str.Substring(token.Length + 1) : string.Empty;
        }
    }
}