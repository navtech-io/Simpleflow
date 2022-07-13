// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Simpleflow.Tests.Functions
{
    public class StringFunctionsTest
    {
        [Fact]
        public void CheckContains()
        {
            // Arrange
            var script =
                @"

                    let hasValue  = $Contains(input: arg.Text, value: ""here"" )
                    let hasValue2 = $Contains(input: arg.Text, value: ""no"" )

                    output hasValue 
                    output hasValue2
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new { Text = "its here" });

            // Assert
            Assert.True((bool)output.Output["hasValue"]);
            Assert.False((bool)output.Output["hasValue2"]);
        }

        [Fact]
        public void CheckStartsWithEndsWith()
        {
            // Arrange
            var script =
                @"

                    let hasValue  = $StartsWith(input: arg.Text, value: ""its"" )
                    let hasValue2 = $EndsWith(input: arg.Text, value: ""here"" )

                    output hasValue 
                    output hasValue2
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new { Text = "its here" });

            // Assert
            Assert.True((bool)output.Output["hasValue"]);
            Assert.True((bool)output.Output["hasValue2"]);
        }

        [Fact]
        public void CheckTrim()
        {
            // Arrange
            var script =
                @"

                    let trim = $Trim(input: arg.Text, value: "" @"" )

                    output trim 
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new { Text = " its here@ " });

            // Assert
            Assert.Equal(expected: "its here", actual: output.Output["trim"]);
        }

        [Fact]
        public void CheckIndexOf()
        {
            // Arrange
            var script =
                @"

                    let index = $IndexOf(input: arg.Text, value: ""here"" )

                    output index
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new { Text = " its here@ " });

            // Assert
            Assert.Equal(expected: 5, actual: output.Output["index"]);
        }

        [Fact]
        public void CheckRegexMatch()
        {
            // Arrange
            var script =
                @"

                    let matched = $match(input: arg.Text, pattern: ""[0-9]*"" )

                    output matched
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new { Text = " its here 995 " });

            // Assert
            Assert.True((bool)output.Output["matched"]);
        }

        [Fact]
        public void CheckSubstring()
        {
            // Arrange
            var script =
                @"

                    let text   = $Substring(input: arg.Text, startIndex: 4 )
                    let text2  = $Substring(input: arg.Text, startIndex: 4, length: 2 )

                    output text
                    output text2
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new { Text = "its here" });

            // Assert
            Assert.Equal(actual: output.Output["text"], expected: "here");
            Assert.Equal(actual: output.Output["text2"], expected: "he");

        }

        [Fact]
        public void CheckStringLength()
        {
            // Arrange
            var script =
                @"

                    let len  = $Length(input: arg.Text)

                    output len
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new { Text = "its here" });

            // Assert
            Assert.Equal(actual: output.Output["len"], expected: 8);
        }

        [Fact]
        public void CheckStringConcat()
        {
            // Arrange
            var script =
                @"

                    let val  = $Concat(value1: arg.Text, value2: "" abc"")

                    output val
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new { Text = "its here" });

            // Assert
            Assert.Equal(actual: output.Output["val"], expected: "its here abc");
        }

    }
}
