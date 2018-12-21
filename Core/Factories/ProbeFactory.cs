using System;
using Lexer.core.Utility;
using Lexer.Core.Interfaces.Probe;

namespace Lexer.core.Factories
{
    public static class ProbeFactory
    {
        public static IProbeFactory<T> New<T>() => new ProbeFactory<T>();
    }

    public class ProbeFactory<T> : IProbeFactory<T>
    {
        private Action<IProbe<T>> _action = _ => { };

        private Action<T> _callback = _ => { };

        private IProbe<T> _instance;

        public IProbe<T> Instance =>
            LambdaHelper.YieldAndAlso(_instance, x => x == null, () => throw new NotImplementedException());

        public IProbeFactory<T> WithAction(Action<IProbe<T>> action)
        {
            _action = action;

            return this;
        }

        public IProbeFactory<T> WithCallback(Action<T> callback)
        {
            _callback = callback;

            return this;
        }

        public IProbe<T> Build()
        {
            return _instance = new Probe<T>(_action, _callback);
        }
    }
}