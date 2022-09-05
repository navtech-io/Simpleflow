// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System;
using Xunit;

namespace Simpleflow.Tests.Scripting
{
    public class TypeCastingTest
    {
        [Fact]
        public void CheckTypeCastingDecimalToIntWithCustomFunc()
        {

            // Arrange
            var script =
                @"
                    let int = 10 # inferred as int
                    let decimal = 20.0 # inferred as decimal

                    # convert decimal to int
                    set int = $decimal_to_int(value: decimal)

                    output int
                ";

            var functionRegister = new FunctionRegister().Add("decimal_to_int", (Func<decimal, int>)DecimalToInt);

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, new object(), functionRegister);

            // Assert
            Assert.Equal(20, output.Output["int"]);
        }

        private static int DecimalToInt(decimal value)
        {
            return Convert.ToInt32(value);
        }
    }
}
