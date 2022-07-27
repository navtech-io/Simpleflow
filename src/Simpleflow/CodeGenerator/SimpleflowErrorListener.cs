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

            if (recognizer.Atn.states[e.OffendingState].StateType == Antlr4.Runtime.Atn.StateType.BlockStart)
            {
                Errors.Add(new SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, $"Unexpected token {offendingSymbol.Text}, a newline expected", e));
            }
            else
            {
                Errors.Add(new SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e));
            }
        }

        public string GetAggregateMessages()
        {
            return string.Join(";\r\n", Errors);
        }
    }
}
