using System.IO;
using System.Text;

namespace Lexer.core.Extensions
{
    public static class StringExtension
    {
        public static string TakeFirst(this string str, string token)
        {
            return str.Length > token.Length ? str.Substring(token.Length + 1) : string.Empty;
        }
        
        public static MemoryStream ToMemoryStream(this string value)
        {
            return new MemoryStream(Encoding.Default.GetBytes(value ?? string.Empty));
        }
    }
}