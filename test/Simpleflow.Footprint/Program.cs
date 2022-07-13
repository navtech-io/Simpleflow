// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Simpleflow.Footprint
{
    [MemoryDiagnoser, HardwareCounters]
    //[DisassemblyDiagnoser(maxDepth: 3)]
    [AllStatisticsColumn]
    [GcServer(true)]
    [InProcess]
    public class SimpleflowVsInline
    {
        private readonly SimpleArgument _context  ;
        private readonly string _flowScript, _flowScript2;

        public SimpleflowVsInline()
        {
            _context = new SimpleArgument() {Id = 233};
            _flowScript =
                @"
                    let x = 233
                    rule when context.Id == x then
                        message ""test""
                ";

            _flowScript2 =
                @"
                let a = 2
                let b = 5
                let text = ""Welcome to new विश्वम्‌""
                let liberate = true
                let date = $GetDate()
                let value = ( 2+3 ) * context.Id - 1  /* 5 x 233 -1 = 1164*/
                    
                rule when  a == 2 then
                    message ""Valid-1""
                
                rule when  ""x"" == text then
                    message ""Valid-xy""
                          
                rule when context.Id == 233 and a == 2 then
                    message ""Valid-2""
                    message ""Valid-3""
                end rule
                
                message ""It works all the time""
                message date

                /* 
                    Change variable, it works only if the mutable option is set to true
                    otherwise it runs as functional scripting
                */

                mutate a = 3

                output a
                output text
                output b
                output context.Id
                output value
                
                rule when (context.Id == 233 and a == 3) or 2 == 3 then
                      error ""Invalid-""
            ";
        }

        [Benchmark]
        public FlowOutput SimpleflowTest()
        {
           return  SimpleflowEngine.Run(_flowScript, _context);
        }

        [Benchmark]
        public FlowOutput SimpleflowWithMoreInstructionsTest()
        {
            return SimpleflowEngine.Run(_flowScript2, _context);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            SimpleflowVsInline s = new SimpleflowVsInline();
            s.SimpleflowTest();
            s.SimpleflowWithMoreInstructionsTest();

            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }

    class SimpleArgument
    {
        public int Id { get; set; }
    }
}
