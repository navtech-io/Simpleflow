// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Antlr4.Runtime;

namespace Simpleflow
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct SyntaxError
    {
        internal readonly IRecognizer Recognizer;
        internal readonly RecognitionException Exception;

        public readonly IToken OffendingSymbol;
        public readonly int Line;
        public readonly int CharPositionInLine;
        public readonly string Message;

        public SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line,
            int charPositionInLine, string message, RecognitionException exception)
        {
            Recognizer = recognizer;
            OffendingSymbol = offendingSymbol;
            Line = line;
            CharPositionInLine = charPositionInLine;
            Message = message;
            Exception = exception;
        }

        public override string ToString()
        {
            var sourceName = Recognizer.InputStream.SourceName;
            return $"{sourceName} Line {Line}:{CharPositionInLine} {Message}";
        }
    }
}
