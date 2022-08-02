// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System.IO;
using Antlr4.Runtime;

namespace Simpleflow.Parser
{

#if DEBUG
    public
#else
    internal
#endif
    abstract class SimpleflowLexerBase : Lexer
    {
        /// templateDepth will be 2. This variable is needed to determine if a `}` is a
        /// plain CloseBrace, or one that closes an expression inside a template string.
        private int _templateDepth = 0;

        protected SimpleflowLexerBase(ICharStream input) : base(input)
        {

        }
        protected SimpleflowLexerBase(ICharStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
        {

        }
      
    }
}
