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
