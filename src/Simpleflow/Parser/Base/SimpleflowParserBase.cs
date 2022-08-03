// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System.IO;
using Antlr4.Runtime;
using static Simpleflow.Parser.SimpleflowParser;

namespace Simpleflow.Parser
{

#if DEBUG
    public
#else
    internal
#endif
    abstract class SimpleflowParserBase : Antlr4.Runtime.Parser
    {
        protected SimpleflowParserBase(ITokenStream input) : base(input)
        {
        }

        protected SimpleflowParserBase(ITokenStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
        {
        }

        protected bool LineTerminatorAhead()
        {
            // Get the token ahead of the current index.
            int possibleIndexEosToken = CurrentToken.TokenIndex -1 ;
            IToken ahead = ((ITokenStream)this.InputStream).Get(possibleIndexEosToken);

            if (ahead.Channel != Lexer.Hidden)
            {
                // We're only interested in tokens on the Hidden channel.
                return false;
            }

            if (ahead.Type == LineTerminator)
            {
                // There is definitely a line terminator ahead.
                return true;
            }

            if (ahead.Type == WhiteSpaces)
            {
                // Get the token ahead of the current whitespaces.
                possibleIndexEosToken = CurrentToken.TokenIndex - 2;
                ahead = ((ITokenStream)this.InputStream).Get(possibleIndexEosToken);
            }

            // Get the token's text and type.
            string text = ahead.Text;
            int type = ahead.Type;

            // Check if the token is, or contains a line terminator.
            return (type == MultiLineComment && (text.Contains("\r") || text.Contains("\n"))) ||
                    (type == LineTerminator);
        }

    }
}
