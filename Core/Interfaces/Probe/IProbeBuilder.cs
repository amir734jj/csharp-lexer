using System;

namespace Lexer.Core.Interfaces.Probe
{
    internal interface IProbeFactory<T>
    {        
        IProbe<T> Instance { get; }

        IProbeFactory<T> WithAction(Action<IProbe<T>> action);
        
        IProbeFactory<T> WithCallback(Action<T> callback);
        
        IProbe<T> Build();
    }
}