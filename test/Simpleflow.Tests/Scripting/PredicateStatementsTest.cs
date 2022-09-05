// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using Xunit;
using Simpleflow.Tests.Helpers;


namespace Simpleflow.Tests.Scripting
{
    public class PredicateStatementsTest
    {
        [Fact]
        public void EqualPredicate()
        {
            // Arrange

            var script =
                @"
                   rule when 1 == 1 and (2==2 or 3 == 2) then
                      message ""1==1""
                ";

            // Act 
            FlowOutput output= SimpleflowEngine.Run(script, new SampleArgument());

            // Assert
            Assert.Equal(actual: output.Messages.Count, expected: 1);
            Assert.Equal(actual: output.Messages[0], expected: "1==1");
        }

        [Fact]
        public void EqualPredicateWithArithmenticExpressiion()
        {
            // Arrange

            var script =
                @"
                   rule when 5 == 2 + 3 then
                      message ""Arithmetic""
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument());

            // Assert
            Assert.Equal(actual: output.Messages.Count, expected: 1);
            Assert.Equal(actual: output.Messages[0], expected: "Arithmetic");
        }

        [Fact]
        public void NonEqualOperators()
        {
            // Arrange

            var script =
                @"
                   rule when 5 > 2 then
                      message '5>2'
                   rule when 5 >= 5 then
                      message '5>=5'
                    rule when 2 < 5 then
                      message '2<5'
                    rule when 3 <= 3 then
                      message '3<=3'
                    rule when 3 != 5 then
                      message '3!=5'
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument());

            // Assert
            Assert.Equal(actual: output.Messages[0], expected: "5>2");
            Assert.Equal(actual: output.Messages[1], expected: "5>=5");
            Assert.Equal(actual: output.Messages[2], expected: "2<5");
            Assert.Equal(actual: output.Messages[3], expected: "3<=3");
            Assert.Equal(actual: output.Messages[4], expected: "3!=5");
        }

        [Fact] 
        public void FunctionInPredicate()
        {
            // Arrange
            
            var script =
                @"
                   rule when not $match(input: ""abc12"", pattern: ""^[a-zA-z]+$"")  then
                        error ""Invalid name. Name should contain alphabet only""
                ";

            FlowOutput output = SimpleflowEngine.Run(script, new SampleArgument());
            
            Assert.Single(output.Errors);
        }

        // TODO write test cases for all types of relational operators
        // TODO write test cases for all types of logical operators

        [Fact]
        public void InOperator()
        {
            // Arrange

            var script =
                @"
                   rule when 5 in [2,3,5] then
                      message '5in2,3,5'

                   rule when not (5 in [2,3]) then
                      message '5notin2,3'
                ";

            // Act 
            FlowOutput output = SimpleflowEngine.Run(script, new object());

            // Assert
            Assert.Equal(actual: output.Messages[0], expected: "5in2,3,5");
            Assert.Equal(actual: output.Messages[1], expected: "5notin2,3");
        }

        [Fact]
        public void InvalidInOperatorUsage()
        {
            // Arrange

            var script =
                @"
                   rule when 5 in 10 then
                      message 'invalid'
                ";

            // Act & Assert
            Assert.Throws<Exceptions.SimpleflowException>(() => SimpleflowEngine.Run(script, new object()));
        }

        [Fact]
        public void CheckDateComparision()
        {
            // Arrange

            var script =
                @"
                   let d1 = $date(y: 2012, m:10, d:10) 
                   let d2 = $date(y: 2011, m:10, d:10) 
                    
                   rule when d2 < d1 then
                       message ""correct"" 
                
                ";

            FlowOutput output = SimpleflowEngine.Run(script, new object());

            Assert.Single(output.Messages);
        }

    }
}
