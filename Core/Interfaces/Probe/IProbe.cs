using System;

namespace Lexer.Core.Interfaces.Probe
{
    public interface IProbe<T>
    {
        T Value { get; }
        
        bool Resolved { get; }
        
        void SetValue(T arg);

        void Trigger();
    }
}