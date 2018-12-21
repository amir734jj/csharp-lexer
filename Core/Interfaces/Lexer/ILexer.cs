using System.Collections.Generic;
using System.IO;

namespace Lexer.Core.Interfaces.Lexer
{
    public interface ILexer<out T> where T: ILexerNode
    {
        IEnumerable<T> Process(MemoryStream stream);
    }
}