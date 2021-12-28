using System;
using Lexer.Core.Interfaces.Probe;

namespace Lexer.core.Utility
{
    internal class Probe<T> : IProbe<T>
    {        
        private readonly Action<T> _callback;
        
        private readonly Action<IProbe<T>> _action;

        public T Value { get; private set; }
        
        public bool Resolved { get; private set; }
        
        public void SetValue(T arg)
        {
            Resolved = true;
            Value = arg;
            
            // Call callback function
            _callback(Value);
        }

        public Probe(Action<IProbe<T>> action, Action<T> callback)
        {
            _action = action;
            _callback = callback;
        }

        public void Trigger()
        {
            _action(this);
        }
    }
}