using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Lexer.core.Enums;
using Lexer.core.Extensions;
using Lexer.core.Factories;
using Lexer.core.Models;
using Lexer.Core.Interfaces.Lexer;

namespace Lexer.core
{
    public class Lexer<T> : ILexer<T> where T : ILexerNode
    {
        private readonly AmbiguityResolverEnum _ambiguityResolver;

        private readonly Func<T> _nil;

        private readonly Dictionary<Regex, Action<ILexerArg<T>>> _handlers;

        public Lexer(AmbiguityResolverEnum ambiguityResolver, Func<T> nil,
            Dictionary<Regex, Action<ILexerArg<T>>> handlers)
        {
            _ambiguityResolver = ambiguityResolver;
            _nil = nil;
            _handlers = handlers;
        }

        public IEnumerable<T> Process(MemoryStream stream)
        {
            var buffer = string.Empty;

            var tokens = new List<T>();

            // Flag indicating continuation of lexing
            var stopFlag = false;

            using (stream)
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream && !stopFlag)
                    {
                        // Append character to buffer
                        buffer += (char) reader.Read();

                        // Hold on to the next character
                        var next = (char) reader.Peek();

                        // Find current matches
                        var currentMatch = _handlers.Where(x => x.Key.FullMatch(buffer)).ToList();

                        // Find matches coming up in the next iteration
                        var futureMatch = _handlers.Where(x => x.Key.FullMatch(buffer + next)).ToList();

                        if (futureMatch.Any() && currentMatch.Any()) continue;

                        if (currentMatch.Any())
                        {
                            var lastToken = tokens.LastOrElse(_nil());

                            var arg = new LexerArg<T>(buffer, next, lastToken,
                                () => StopAction(ref stopFlag),
                                // ReSharper disable once AccessToModifiedClosure
                                (str, token) => YieldAction(ref buffer, ref tokens, str, token),
                                str => DisposeAction(ref buffer, str),
                                str => SetBufferAction(ref buffer, str)
                            );

                            // Invoke handler
                            ResolveHandler(_ambiguityResolver, buffer, next, lastToken, currentMatch).Value(arg);
                        }
                        else
                        {
                            throw new Exception("Lexer failed: " +
                                                $"{Environment.NewLine} Buffer: {buffer}" +
                                                $"{Environment.NewLine} Next: {next}");
                        }
                    }
                }
            }

            // Return tokens
            return tokens;
        }

        private static KeyValuePair<Regex, Action<ILexerArg<T>>> ResolveHandler(
            AmbiguityResolverEnum ambiguityResolverEnum, string buffer, char next, T lastToken,
            // ReSharper disable once ParameterTypeCanBeEnumerable.Local
            List<KeyValuePair<Regex, Action<ILexerArg<T>>>> handlers)
        {
            switch (ambiguityResolverEnum)
            {
                case AmbiguityResolverEnum.FirstMatch:
                    return handlers.First();
                case AmbiguityResolverEnum.LastMatch:
                    return handlers.Last();
                case AmbiguityResolverEnum.LongestMatch:
                    var handlerAny = handlers
                        .Select(x => new
                        {
                            Handler = x,
                            Factory = ProbeFactory.New<string>()
                        })
                        .Select(x => new
                        {
                            // ReSharper disable once AccessToModifiedClosure
                            x.Factory, x.Handler, Arg = new LexerArg<T>(buffer, next, lastToken,
                                // Test should not be able to stop the parser
                                () => { },
                                (a, b) => x.Factory.Instance.SetValue(a),
                                _ => { },
                                _ => { }
                            )
                        })
                        .SelectAndAlso(x => new
                        {
                            x.Factory,
                            x.Handler,
                            x.Arg,
                            Probe = x.Factory.Build()
                        }, (x, y) => x.Handler.Value(x.Arg))
                        .SelectAndAlso(x => new
                        {
                            x.Probe,
                            x.Handler
                        }, (x, y) => x.Probe.Trigger())
                        .Where(x => x.Probe.Resolved)
                        .OrderBy(x => x.Probe.Value.Length)
                        .LastOrDefault();

                    return handlerAny?.Handler ?? ResolveHandler(AmbiguityResolverEnum.FirstMatch, buffer, next,
                               lastToken, handlers);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // ReSharper disable once RedundantAssignment
        private static void SetBufferAction(ref string oldBuffer, string newBuffer)
        {
            oldBuffer = newBuffer;
        }

        // ReSharper disable once RedundantAssignment
        private static void StopAction(ref bool stop)
        {
            stop = true;
        }

        private static void DisposeAction(ref string buffer, string str)
        {
            // Filter-out the buffer
            // ReSharper disable once AccessToModifiedClosure
            buffer = buffer.TakeFirst(str);
        }

        private static void YieldAction(ref string buffer, ref List<T> tokens, string str, T token)
        {
            // Filter-out the buffer
            // ReSharper disable once AccessToModifiedClosure
            buffer = buffer.TakeFirst(str);

            tokens = tokens.Concat(new[] {token}).ToList();
        }
    }
}