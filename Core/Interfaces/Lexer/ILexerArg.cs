using System;

namespace Lexer.Core.Interfaces.Lexer
{
    public interface ILexerArg<T>
    {
        string Buffer { get; }
        
        char Next { get; }
        
        T Previous { get; }
        
        Action Stop { get; }
        
        Action<string, T> Yield { get; }

        Action Nothing { get; }
        
        Action<string> Dispose { get; }
        
        Action<string> SetBuffer { get; }
    }
}