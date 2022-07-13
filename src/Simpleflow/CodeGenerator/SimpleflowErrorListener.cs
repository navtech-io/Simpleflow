// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.IO;
using System.Collections.Generic;

using Antlr4.Runtime;

namespace Simpleflow.CodeGenerator
{
    internal class SimpleflowErrorListener : BaseErrorListener
    {
        public readonly List<SyntaxError> Errors = new List<SyntaxError>();


        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg, RecognitionException e)
        {
            Errors.Add(new SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e));


            //base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        }

        public string GetAggregateMessages()
        {
            return string.Join(';', Errors);
        }
    }
}
