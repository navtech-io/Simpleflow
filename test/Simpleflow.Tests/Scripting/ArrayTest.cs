// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System.Collections.Generic;
using Xunit;

using Simpleflow.Tests.Helpers;

namespace Simpleflow.Tests.Scripting
{
    public class ArrayTest
    {
        [Fact]
        public void SingleDimensionIntegerArray()
        {
            // Arrange
            var arg = new SampleArgument { Id = 10 };
            var script =
                @"
                    let ar = [1, 2, 4, 5]
                    output ar
                ";

            // Act 
            var result = SimpleflowEngine.Run(script, arg);

            // Assert
            Assert.IsType<List<int>>(result.Output["ar"]);

            var list = (result.Output["ar"] as List<int>);
            
            Assert.Equal(4, (list).Count);
            Assert.Collection(list, 
                             (item)=> Assert.Equal(1, item),
                             (item) => Assert.Equal(2, item),
                             (item) => Assert.Equal(4, item),
                             (item) => Assert.Equal(5, item)
                             );
        }

        [Fact]
        public void SingleDimensionStringArray()
        {
            // Arrange
            var arg = new SampleArgument { Id = 10 };
            var script =
                @"
                    let ar = [""test"", ""abc"", `xyz`]
                    output ar
                ";

            // Act 
            var result = SimpleflowEngine.Run(script, arg);

            // Assert
            Assert.IsType<List<string>>(result.Output["ar"]);

            var list = (result.Output["ar"] as List<string>);

            Assert.Equal(3, (list).Count);
            Assert.Collection(list,
                             (item) => Assert.Equal("test", item),
                             (item) => Assert.Equal("abc", item),
                             (item) => Assert.Equal("xyz", item)
                             );
        }


        [Fact]
        public void SingleDimensionObjectArray()
        {
            // Arrange
            var arg = new SampleArgument { Id = 10 };
            var script =
                @"
                    let ar = [1, ""abc"", `xyz`, 2.5]
                    output ar
                ";

            // Act 
            var result = SimpleflowEngine.Run(script, arg);

            // Assert
            Assert.IsType<List<object>>(result.Output["ar"]);

            var list = (result.Output["ar"] as List<object>);

            Assert.Equal(4, (list).Count);
            Assert.Collection(list,
                             (item) => Assert.Equal(1, item),
                             (item) => Assert.Equal("abc", item),
                             (item) => Assert.Equal("xyz", item),
                             (item) => Assert.Equal(2.5M, item)
                             );
        }

        [Fact]
        public void AccessArrayValue()
        {
            // Arrange
            var arg = new int[] { 1, 2, 3 };
            var script =
                @"
                 let i = 1
                 let x=arg [ i ]
                 output x

                ";

            // Act 
            var result = SimpleflowEngine.Run(script, arg);

            // Assert
            Assert.Equal(2, result.Output["x"]);
        }
    }
}
