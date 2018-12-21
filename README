# C# Lexer

Simple lexer written in C#

### Code example:
```csharp
// Class that extends ILexerNode
var lexer = LexerBuilder.New<BaseNode>()
    .Init()
    .WithAmbiguityResolverEnum(AmbiguityResolverEnum.FirstMatch)
    // Define the nil node
    .NilNode(() => new NilNode())
    .WithHandler(new Regex("[\\d]+"), opt => {
      opt.Yield(opt.Buffer, new NumericNode {Value = decimal.Parse(opt.Buffer)});
    })
    .WithHandler(new Regex("[+-\\/*]"), opt => {
      opt.Yield(opt.Buffer, new OpNode {Operator = opt.Buffer[0]});
    })
    .WithHandler(new Regex("[\\s]+"), opt => {
        opt.Dispose(opt.Buffer);
    })
    .WithHandler(new Regex("[\\w]+"), opt => opt.SetBuffer(string.Empty))
    .WithHandler(new Regex("$"), opt => opt.Stop())
    .FinalizeHandlers()
    .Build();

var nodes = lexer.Process("12 * 23 + 34 / 45 - 56 Error".ToMemoryStream());

> nodes
// NumericNode, OpNode, and etc.
```

### Notes:
  - Order of handlers matter in case of FirstMatch or LastMatch
  - Handler can yield more than one time
  - Upon invocation of Yield, the string gets removed from the beginning of the buffer
  - Handler has option (i.e. `opt`):
    - Buffer string
    - Next character
    - Previous token
    - Stop Lexer
    - Modify Buffer
    - Yield result
    - Dispose a string from the beginning of the Buffer
    - Do nothing
  - AmbiguityResolvers:
    - `FirstMatch`: takes the first matching handler
    - `LastMatch`: takes the last matching handler
    - `LongestMatch`: takes the handler yielding handler (defaults back to `FirstMatch` in case of failure)
