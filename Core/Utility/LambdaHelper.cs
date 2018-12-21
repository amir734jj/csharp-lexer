using System;

namespace Lexer.core.Utility
{
    public static class LambdaHelper
    {
        public static T YieldAndAlso<T>(T value, Func<T, bool> when, Action also)
        {
            // Call also when ...
            if (when(value))
            {
                also();
            }

            return value;
        }
    }
}