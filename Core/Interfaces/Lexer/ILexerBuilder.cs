using System;
using System.Text.RegularExpressions;
using Lexer.core.Enums;

namespace Lexer.Core.Interfaces.Lexer
{
    public interface ILexerBuilder<T> where T: ILexerNode
    {
        ILexerBuilderAmbiguityResolver<T> Init();
    }
    
    public interface ILexerBuilderAmbiguityResolver<T> where T: ILexerNode
    {
        ILexerBuilderWithNilNode<T> WithAmbiguityResolverEnum(AmbiguityResolverEnum ambiguityResolver);
    }
    
    public interface ILexerBuilderWithNilNode<T> where T: ILexerNode
    {
        ILexerBuilderHandlers<T> NilNode(Func<T> nil);
    }
    
    public interface ILexerBuilderHandlers<T> where T: ILexerNode
    {
        ILexerBuilderHandlers<T> WithHandler(Regex regex, Action<ILexerArg<T>> handler);

        ILexerBuilderFinalize<T> FinalizeHandlers();
    }
    
    public interface ILexerBuilderFinalize<T> where T: ILexerNode
    {
        ILexer<T> Build();
    }
}