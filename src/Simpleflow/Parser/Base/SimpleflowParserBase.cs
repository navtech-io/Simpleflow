// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System.IO;
using Antlr4.Runtime;

namespace Simpleflow.Parser
{
    internal abstract class SimpleflowParserBase : Antlr4.Runtime.Parser
    {
        protected SimpleflowParserBase(ITokenStream input) : base(input)
        {
        }

        protected SimpleflowParserBase(ITokenStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
        {
        }
    }
}
