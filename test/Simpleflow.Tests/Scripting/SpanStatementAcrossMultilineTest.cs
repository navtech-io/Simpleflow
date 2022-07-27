// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Linq;
using System.Collections.Generic;
using Xunit;

using Simpleflow.CodeGenerator;
using Simpleflow.Tests.Helpers;
using Simpleflow.Exceptions;


namespace Simpleflow.Tests.Scripting
{
    public class SpanStatementAcrossMultilineTest
    {


        [Fact]
        public void SpanStatementAcrossMultiline()
        {
            // Arrange
            var script =
                @"
                  let data = {
                                a : ""test_program"" ,
                                b: 233
                             } /* working */

                  let test = true 
                  rule when test 
                                then  
                        message ""done""

                ";

            // Act 
            var result = SimpleflowEngine.Run(script, new SampleArgument());

            // Assert
            Assert.Equal("done", result.Messages[0]);
        }

        [Fact]
        public void InvalidStatementAcrossMultiline()
        {
            // Arrange
            var script =
                @"
                  let data = {
                                a : ""sadasds"" ,
                                b: 233
                             } let test = true
                  rule when test 
                                then  
                        message ""done""

                ";

            // Act & Assert
            AssertEx.Throws<SyntaxException>(s =>
                {
                    Assert.Contains("newline expected", s.Message);
                }, 
                () => SimpleflowEngine.Run(script, new SampleArgument())
            );
        }
    }
}
