// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Tree;
using Simpleflow.Parser;

namespace Simpleflow.Ast
{
    internal partial class SimpleflowAstVisitor : SimpleflowParserBaseVisitor<BlockNode>
    {
        public override BlockNode Visit(IParseTree tree)
        {
            BlockNode node = new BlockNode() { Children = new List<BlockNode>() };

            /* Process each statement */
            for (int i = 0; i < tree.ChildCount; i++)
            {
                var c = tree.GetChild(i);
                var childResult = Visit(c);

                if (childResult != null)
                {
                    node.Children.Add(childResult);
                }
            }
            node.Text = node.Children.Count == 0 ? tree.GetText() : GetStatement(node);
            return node;
        }

        public string GetStatement(BlockNode node)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < node.Children.Count; i++)
            {
                var st = node.Children[i].Text;
                sb.Append(st + " ");
            }
            return sb.ToString();
        }
    }
}
