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
    public class LetStatementTest
    {

        [Theory]
        [MemberData(nameof(Data))]
        public void InvalidVariableName(string value1)
        {
            // Arrange
            var script =
                @$"
                  let {value1} = 5
                  message arg
                ";

            // Act & Assert
            Assert.Throws<VariableNameViolationException>(
                () => SimpleflowEngine.Run(script, new SampleArgument()));
        }

        [Fact]
        public void CaseInsensitiveVariableName()
        {
            // Arrange
            var arg = new SampleArgument { Id = 10 };
            var script =
                @$"
                  let Value = arg.Id
                  set value =  value + 10
                  message value
                ";

            // Act & Assert
            var output = SimpleflowEngine.Run(script, arg);
            Assert.Equal("20", output.Messages[0]);
        }

        [Fact]
        public void DiscardLetVariable()
        {
            // Arrange
            var arg = new SampleArgument();
            var script =
                @"
                  let _ = 2  # ignore
                  let _, err = 2 / 0  # ignore

                ";

            // Act & Assert (No Error)
            var output = SimpleflowEngine.Run(script, arg);
            
        }

        [Fact]
        public void DiscardLetVariableNotAllowedForJsonObj()
        {
            // Arrange
            
            var script =
                @"
                  let _ = 2  # ignore
                  let _, err = {}

                ";

            // Act & Assert
            AssertEx.Throws<SimpleflowException>(
                    Resources.Message.CannotIgnoreIdentifierForJsonObj,
                    () => SimpleflowEngine.Run(script, new object())
                );
        }

        [Fact]
        public void DiscardSetVariable()
        {
            // Arrange
            var arg = new SampleArgument();
            var script =
                @"
                  set _ = 2  # ignore
                  set _, err = 2 / 0  # ignore

                ";

            // Act & Assert (No Error)
            var output = SimpleflowEngine.Run(script, arg);
        }

        [Fact]
        public void DiscardSetVariableNotAllowedForJsonObj()
        {
            // Arrange

            var script =
                @"
                  set _, err = {}
                ";

            // Act & Assert
            AssertEx.Throws<SimpleflowException>(
                    Resources.Message.CannotIgnoreIdentifierForJsonObj,
                    () => SimpleflowEngine.Run(script, new object())
                );
        }

        [Fact]
        public void DiscardPartialSetVariableNotAllowedForJsonObj()
        {
            // Arrange

            var script =
                @"
                  partial set _, err = {}
                ";

            // Act & Assert
            AssertEx.Throws<SimpleflowException>(
                    Resources.Message.CannotIgnoreIdentifierForJsonObj,
                    () => SimpleflowEngine.Run(script, new object())
                );
        }


        [Fact]
        public void LetIncorrectSyntax()
        {
            // Arrange
            var script =
                @"
                  let aa@a = 23
                  message aa@a
                ";
            
            // Act & Assert
            Assert.Throws<SyntaxException>(() => SimpleflowEngine.Run(script, new SampleArgument()));
        }

        [Fact]
        public void CheckDuplicateVariableDeclarationException()
        {
            // Arrange
            var script =
                @"
                  let test = 23
                  let test = true

                ";

            // Act & Assert
            AssertEx.Throws<DuplicateVariableDeclarationException>(
                ex => Assert.Equal("test", ex.VariableName),
                () => SimpleflowEngine.Run(script, new SampleArgument()));
        }

        public static IEnumerable<object[]> Data() => SimpleflowKeywords.Keywords.Select(k => new object[] { k });
    }
}
