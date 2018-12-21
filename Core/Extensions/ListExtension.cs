using System;
using System.Collections.Generic;

namespace Lexer.core.Extensions
{
    public static class ListExtension
    {
        public static T FirstOrElse<T>(this List<T> list, T node)
        {
            return list.Count > 0 ? list[0] : node;
        }
        
        public static T LastOrElse<T>(this List<T> list, T node)
        {
            return list.Count > 0 ? list[list.Count - 1] : node;
        }
    }
}