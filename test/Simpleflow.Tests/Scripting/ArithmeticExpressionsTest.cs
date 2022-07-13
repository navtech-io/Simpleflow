// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;

using Simpleflow.CodeGenerator;
using Simpleflow.Tests.Helpers;

namespace Simpleflow.Tests.Scripting
{
    public class ArithmeticExpressionsTest
    {
        [Fact]
        public void ArithmeticOperations()
        {
            // Arrange
            var context = new SampleArgument() { Id = 10};
            // TODO negative and with different data types (int,float,decimal)
            // TODO module % operator 

            var script =
                @"
                    let value  =   2 + 3   * 2 - 1
                    let value2 = ( 2 + 3 ) * 2 - 1
                    let value3 = ( 2 + 3 ) * arg.Id - 1
                    let value4 = ( 2 + 3 * 2 ) * 2 - 1
                    let value5 = ( (4 + 3) * 2 ) * 2 - 1
                    let value6 = (  4 + 3 * 2 ) * 2 - 1

                    output value  
                    output value2
                    output value3
                    output value4
                    output value5
                    output value6
                ";

            // Act
            FlowOutput output = SimpleflowEngine.Run<SampleArgument>(script, context);

            // Assert
            Assert.Equal(actual: output.Output.Count, expected: 6);

            Assert.Equal(actual: output.Output["value"], expected: 7);
            Assert.Equal(actual: output.Output["value2"], expected: 9);
            Assert.Equal(actual: output.Output["value3"], expected: 49);
            Assert.Equal(actual: output.Output["value4"], expected: 15);

            Assert.Equal(actual: output.Output["value5"], expected: 27);
            Assert.Equal(actual: output.Output["value6"], expected: 19);
        }


        [Fact]
        public void CheckMixedTypeExpression()
        {
          
            // Arrange
            var script =
                @"
                    let x = 10.5 - arg.Value
                    let y = -10.5 + 10

                    output x
                    output y
                ";

            var context = new SampleArgument { Value = 5.0M };


            // Act
            FlowOutput output = SimpleflowEngine.Run(script, context);

            // Assert
            Assert.Equal(expected: 5.5M, actual: output.Output["x"]);
            Assert.Equal(expected: -0.5M, actual: output.Output["y"]);

        }

        [Fact]
        public void CheckAddition()
        {
            // Arrange
            var script =
                @"
                    let w = 2 + 3 + arg.Value
                    
                    output w
                ";

            var context = new SampleArgument { Value = 5.5M };


            // Act
            FlowOutput output = SimpleflowEngine.Run(script, context);

            Assert.Equal(expected: 10.5M, actual: output.Output["w"]);

        }

    }


}
