// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Simpleflow.Tests.Functions
{
    public class DataTypeConversionFunctions
    {
        [Fact]
        public void StrConversion()
        {
            // Arrange
            var script =
                @"
                    let v   = $str(value: 2)
                    let v1  = $str(value: false)
                    let v2  = $str(value: 'as')
                    let v3  = $str(value: 2.3)

                    output v
                    output v1
                    output v2
                    output v3
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new object());

            // Assert
            Assert.IsType<string>(output.Output["v"]);
            Assert.IsType<string>(output.Output["v1"]);
            Assert.IsType<string>(output.Output["v2"]);
            Assert.IsType<string>(output.Output["v3"]);
        }
    }
}
