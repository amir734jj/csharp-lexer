using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Lexer.core.Enums;
using Lexer.core.Models;
using Lexer.Core.Interfaces.Lexer;

namespace Lexer.core.Builders
{
    public static class LexerBuilder
    {
        public static ILexerBuilder<TSource> New<TSource>() where TSource : class =>
            new LexerBuilder<TSource>();
    }

    public class LexerBuilder<T> : ILexerBuilder<T> where T : class
    {
        public ILexerBuilderAmbiguityResolver<T> Init()
        {
            return new LexerBuilderAmbiguityResolver<T>();
        }
    }

    public class LexerBuilderAmbiguityResolver<T> : ILexerBuilderAmbiguityResolver<T> where T : class
    {
        public ILexerBuilderWithNilNode<T> WithAmbiguityResolverEnum(AmbiguityResolverEnum ambiguityResolver)
        {
            return new LexerBuilderWithNilNode<T>(ambiguityResolver);
        }
    }

    public class LexerBuilderWithNilNode<T> : ILexerBuilderWithNilNode<T> where T : class
    {
        private readonly AmbiguityResolverEnum _ambiguityResolver;

        public LexerBuilderWithNilNode(AmbiguityResolverEnum ambiguityResolver)
        {
            _ambiguityResolver = ambiguityResolver;
        }

        public ILexerBuilderHandlers<T> NilNode(Func<T> nil)
        {
            return new LexerBuilderHandlers<T>(_ambiguityResolver, nil);
        }
    }

    public class LexerBuilderHandlers<T> : ILexerBuilderHandlers<T> where T : class
    {
        private readonly AmbiguityResolverEnum _ambiguityResolver;

        private readonly Func<T> _nil;

        private readonly Dictionary<Regex, Action<ILexerArg<T>>> _handlers;

        public LexerBuilderHandlers(AmbiguityResolverEnum ambiguityResolver, Func<T> nil)
        {
            _ambiguityResolver = ambiguityResolver;
            
            _nil
             = nil;
            
            _handlers = new Dictionary<Regex, Action<ILexerArg<T>>>();
        }

        public ILexerBuilderHandlers<T> WithHandler(Regex regex, Action<ILexerArg<T>> handler)
        {
            _handlers[regex] = handler;

            return this;
        }

        public ILexerBuilderFinalize<T> FinalizeHandlers()
        {
            return new LexerBuilderFinalize<T>(_ambiguityResolver, _nil, _handlers);
        }
    }

    public class LexerBuilderFinalize<T> : ILexerBuilderFinalize<T> where T : class
    {
        private readonly AmbiguityResolverEnum _ambiguityResolver;

        private readonly Func<T> _nil;

        private readonly Dictionary<Regex, Action<ILexerArg<T>>> _handlers;

        public LexerBuilderFinalize(AmbiguityResolverEnum ambiguityResolver, Func<T> nil,
            Dictionary<Regex, Action<ILexerArg<T>>> handlers)
        {
            _ambiguityResolver = ambiguityResolver;
            
            _nil = nil;
            
            _handlers = handlers;
        }

        public ILexer<T> Build()
        {
            return new Lexer<T>(_ambiguityResolver, _nil, _handlers);
        }
    }
}