// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System.Collections.Generic;

namespace Simpleflow.Ast
{
    public class BlockNode
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public List<BlockNode> Children { get; set; }
    }
}
