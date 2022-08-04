// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System.Collections.Generic;

namespace Simpleflow.Ast
{
    public class SyntaxTree : BlockNode
    {
        public IEnumerable<SyntaxError> SyntaxErrors { get; set; }
    }
}
