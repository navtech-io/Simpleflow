// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Simpleflow.Tests.Helpers;
using Xunit;

namespace Simpleflow.Tests.Scripting
{
    public class ParserRefactorTest
    {
        [Fact]
        public void AllExpressionsTest()
        {
            // Arrange
            var context = new SampleArgument() { Id = 10 };

            var script =
                @"
                    let value  = 2 * 3
                    message value + 2
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run<SampleArgument>(script, context);

            // Assert
        }
    }
}
