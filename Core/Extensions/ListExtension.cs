using System;
using System.Collections.Generic;

namespace Lexer.core.Extensions
{
    internal static class ListExtension
    {
        public static T LastOrElse<T>(this List<T> list, T node)
        {
            return list.Count > 0 ? list[list.Count - 1] : node;
        }
    }
}