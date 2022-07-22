---
layout: default
title: API
nav_order: 3
---
# API

1. [Simpleflow Execution](#simpleflow-pipeline)
1. [Flowoutput](#flowoutput)
1. [Register Custom Functions](#register-custom-functions)
1. [Extensibility](#extensibility)
1. [Compile Script](#compile-script)
1. [Control Functions Execution Permissions](#control-functions-execution-permissions)

## Simpleflow Execution
<a name="simpleflow-pipeline"></a>

![Simpleflow Pipeline](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/navtech-io/Simpleflow/main/SimpleflowDiagram.puml)

Sample code to create, build and run pipeline
```csharp
// Build Pipeline to get full control over execution
ISimpleflowPipelineBuilder engine
    = new SimpleflowPipelineBuilder()
            // Add middleware services 
            .AddPipelineServices(
                    new MyPreprocessor(), 
                    new MyLoggingService());
            // Add core services - Cache, Compiler, Execution
            .AddCorePipelineServices(FunctionRegister.Default) // or
            /*
                 AddPipelineServices(
                    new CacheService(), //Cache uses Sliding window cache policy 
                    new CompilerService(ActivityRegister.Default),
                    new ExecutionService()); 
            */
// Build
ISimpleflow flow = engine.Build();
// Run
FlowOutput result = flow.Run(script, new Member { Id = id});
```
## FlowOutput

Emitters (`message, error, output`) produce output from script that will be available in FlowOutput object.

## Register Custom Functions

```csharp
FunctionRegister.Default
    .Add("DerivativeOfXPowN", (Func<int, int, int>)CalcDerivativeOfXPowN)
static int CalcDerivativeOfXPowN(int x, int n)
{
    return n *  Math.Pow(x, n-1); //
}
```
## Extensibility

Create middleware and add it to pipeline.

```csharp
public class LoggingService : IFlowPipelineService
{
    public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
    {
        // log here    
        next?.Invoke(context);
    }
}
```

## Compile Script
By adding only CompilerService to build pipeline, script can be compiled and reported if there are any errors.
```csharp
var engine
    = new SimpleflowPipelineBuilder()
        .AddPipelineServices(new Services.CompilerService(FunctionRegister.Default));
var simpleflow = engine.Build();
try 
{
    simpleflow.Run(script, context);
} 
catch(SimpleflowException exception)
{
    //Handle script errors
}
```

## Control Functions Execution Permissions
By setting FlowContextOptions, you can permit specific functions to be executed.

```csharp
 string script = @"
                    let d = $GetCurrentDateTime()
                    message d
                ";

var output = SimpleflowEngine.Run( script, 
                                   new object(), 
                                   new FlowContextOptions
                                       {
                                         AllowFunctions = new string[] { 
                                                    "GetCurrentDateTime" 
                                            }
                                       }
                                 );
```
