// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Xunit;

using Simpleflow.CodeGenerator;
using Simpleflow.Tests.Helpers;

namespace Simpleflow.Tests.Scripting
{
    public class DataTypesTest
    {
        [Fact]
        public void CheckAllDataTypes()
        {
           
            // Arrange
            var script =
                @"
                    let int = 10
                    let decimal = 10.5
                    let text = ""TEXT""
                    let boolean = true

                    output int
                    output decimal
                    output text
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument());

            // Assert
            Assert.IsType<int>(output.Output["int"]);
            Assert.IsType<decimal>(output.Output["decimal"]);
            Assert.IsType<string>(output.Output["text"]);

        }

    }
}
