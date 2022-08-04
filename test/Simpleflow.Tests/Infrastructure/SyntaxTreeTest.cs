// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using Xunit;

using Simpleflow.Ast;
using Simpleflow.Tests.Helpers;


namespace Simpleflow.Tests.Infrastructure
{
    public class SyntaxTreeTest
    {
        [Fact]
        public void SimpleflowAst()
        {
            // Arrange
            var flowScript =
            @$" 
                /* Declare and initialize variables */
                let userId      = none
                let currentDate = $GetCurrentDateTime ( 
                                        timezone: ""{TestsHelper.Timezone}"" )

                /* Define Rules */
                rule when  arg.Name == """" 
                           or arg.Name == none then
                    error ""Name cannot be empty""


                rule when not $match(input: arg.Name , 
                                     pattern: ""^[a-zA-z]+$"") then
                    error ""Invalid name. Name should contain only alphabets.""


                rule when arg.Age < 18 and arg.Country == ""US"" then
                    error ""You cannot register""
                end rule

                /* Statements outside of the rules */
                message ""validations-completed""

                rule when context.HasErrors then
                    exit
                end rule

                /* Set current date time */
                partial set arg = {{ 
                                     RegistrationDate: currentDate, 
                                     IsActive: true 
                                   }}

            ";

            // Act
            var ast = SimpleflowScript.GetAbstractSyntaxTree(flowScript);

            // Assert
            Assert.Equal(9, ast.Children.Count);
            Assert.Empty(ast.SyntaxErrors);
        }


        [Fact]
        public void SimpleflowAstWithErrors()
        {
            // Arrange
            var flowScript =
            @$" 
                let userId      = none
                test xyz
            ";

            // Act
            var ast = SimpleflowScript.GetAbstractSyntaxTree(flowScript);

            // Assert
            Assert.Single(ast.SyntaxErrors);
        }
    }
}
