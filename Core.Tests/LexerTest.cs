using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Lexer.core.Builders;
using Lexer.core.Enums;
using Lexer.core.Extensions;
using Lexer.Core.Tests.Models;
using Xunit;

namespace Lexer.Core.Tests
{
    public class LexerTest
    {
        [Fact]
        public void Test__FirstMatch()
        {
            // Arrange
            var lexer = LexerBuilder.New<BaseNode>()
                .Init()
                .WithAmbiguityResolverEnum(AmbiguityResolverEnum.FirstMatch)
                .NilNode(() => new NilNode())
                .WithHandler(new Regex("[\\w]+"), opt => opt.Yield(opt.Buffer, new ValueNode {Value = opt.Buffer}))
                .WithHandler(new Regex("[\\s]+"), opt => opt.Yield(opt.Buffer, new SpaceNode()))
                .WithHandler(new Regex("$"), opt => opt.Stop())
                .FinalizeHandlers()
                .Build();

            // Act
            var nodes = lexer.Process(new MemoryStream(Encoding.Default.GetBytes("Hello World")));

            // Assert
            Assert.Equal(3, nodes.Count());
        }
        
        [Fact]
        public void Test__LastMatch()
        {
            // Arrange
            var lexer = LexerBuilder.New<BaseNode>()
                .Init()
                .WithAmbiguityResolverEnum(AmbiguityResolverEnum.LastMatch)
                .NilNode(() => new NilNode())
                .WithHandler(new Regex("[\\w]+"), opt => opt.Yield(opt.Buffer, new ValueNode {Value = opt.Buffer}))
                .WithHandler(new Regex("[\\s]+"), opt => opt.Yield(opt.Buffer, new SpaceNode()))
                .WithHandler(new Regex("$"), opt => opt.Stop())
                .FinalizeHandlers()
                .Build();

            // Act
            var nodes = lexer.Process(new MemoryStream(Encoding.Default.GetBytes("Hello World")));

            // Assert
            Assert.Equal(3, nodes.Count());
        }
        
        [Fact]
        public void Test__LongestMatch()
        {
            // Arrange
            var lexer = LexerBuilder.New<BaseNode>()
                .Init()
                .WithAmbiguityResolverEnum(AmbiguityResolverEnum.LongestMatch)
                .NilNode(() => new NilNode())
                .WithHandler(new Regex("[\\w]+"), opt => opt.Yield(opt.Buffer, new ValueNode {Value = opt.Buffer}))
                .WithHandler(new Regex("[\\s]+"), opt => opt.Yield(opt.Buffer, new SpaceNode()))
                .WithHandler(new Regex("$"), opt => opt.Stop())
                .FinalizeHandlers()
                .Build();

            // Act
            var nodes = lexer.Process(new MemoryStream(Encoding.Default.GetBytes("Hello World")));

            // Assert
            Assert.Equal(3, nodes.Count());
        }
        
        [Fact]
        public void Test__Numeric()
        {
            // Arrange
            var lexer = LexerBuilder.New<BaseNode>()
                .Init()
                .WithAmbiguityResolverEnum(AmbiguityResolverEnum.FirstMatch)
                .NilNode(() => new NilNode())
                .WithHandler(new Regex("[\\d]+"), opt => opt.Yield(opt.Buffer, new NumericNode {Value = decimal.Parse(opt.Buffer)}))
                .WithHandler(new Regex("[+-\\/*]"), opt => opt.Yield(opt.Buffer, new OpNode { Operator = opt.Buffer[0]}))
                .WithHandler(new Regex("[\\s]+"), opt => opt.Dispose(opt.Buffer))
                .WithHandler(new Regex("[\\w]+"), opt => opt.SetBuffer(string.Empty))
                .WithHandler(new Regex("$"), opt => opt.Stop())
                .FinalizeHandlers()
                .Build();

            // Act
            var nodes = lexer.Process(new MemoryStream(Encoding.Default.GetBytes("12 * 23 + 34 / 45 - 56 Error 12 * 23 + 34 / 45 - 56")));

            // Assert
            Assert.Equal(18, nodes.Count());
        }
    }
}