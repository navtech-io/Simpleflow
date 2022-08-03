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
        private int _templateDepth = 0;

        protected SimpleflowLexerBase(ICharStream input) : base(input)
        {

        }
        protected SimpleflowLexerBase(ICharStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
        {

        }

        public bool IsInTemplateString()
        {
            return _templateDepth > 0;
        }

        public void IncreaseTemplateDepth()
        {
            _templateDepth++;
        }

        public void DecreaseTemplateDepth()
        {
            _templateDepth--;
        }


    }
}
