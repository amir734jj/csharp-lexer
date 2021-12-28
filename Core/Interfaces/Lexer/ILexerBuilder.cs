using System;
using System.Text.RegularExpressions;
using Lexer.core.Enums;

namespace Lexer.Core.Interfaces.Lexer
{
    public interface ILexerBuilder<T> where T: class
    {
        ILexerBuilderAmbiguityResolver<T> Init();
    }
    
    public interface ILexerBuilderAmbiguityResolver<T> where T: class
    {
        ILexerBuilderWithNilNode<T> WithAmbiguityResolverEnum(AmbiguityResolverEnum ambiguityResolver);
    }
    
    public interface ILexerBuilderWithNilNode<T> where T: class
    {
        ILexerBuilderHandlers<T> NilNode(Func<T> nil);
    }
    
    public interface ILexerBuilderHandlers<T> where T: class
    {
        ILexerBuilderHandlers<T> WithHandler(Regex regex, Action<ILexerArg<T>> handler);

        ILexerBuilderFinalize<T> FinalizeHandlers();
    }
    
    public interface ILexerBuilderFinalize<out T> where T: class
    {
        ILexer<T> Build();
    }
}