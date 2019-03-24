using System;
using Lexer.Core.Interfaces.Lexer;

namespace Lexer.core.Models
{
    public class LexerArg<T> : ILexerArg<T>
    {
        public LexerArg(string buffer, char next, T previous, Action stop, Action<string, T> yield,
            Action<string> dispose, Action<string> setBuffer)
        {
            Buffer = buffer;
            Next = next;
            Previous = previous;
            Stop = stop;
            Yield = yield;
            Dispose = dispose;
            SetBuffer = setBuffer;
        }

        public string Buffer { get; }
        
        public char Next { get; }
        
        public T Previous { get; }
        
        public Action Stop { get; }
        
        public Action<string, T> Yield { get; }
        
        public Action<string> Dispose { get; }
        
        public Action<string> SetBuffer { get; }
        
        public Action Nothing => () => { };
    }
}
