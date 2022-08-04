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
1. [Function Permissions](#function-permissions)
1. [Register Functions at Context Level](#register-functions-at-context-level)
1. [Cache Options](#cache-options)
1. [Get Syntax Tree](#get-syntax-tree)

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

## Functions Permissions
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

## Register Functions at Context Level

```csharp
string id = "7bfd56c8ca354307b6cb9e0805a7ae4c";

var options = new FlowContextOptions { 
    Id = id,
};

// Register additional function to this context only.
var funcRegister = new FunctionRegister();
funcRegister.Add("GetSysDate", (Func<string>)GetDate);
            .Add("GetCurrentDate", (Func<string>)GetDate);  // Override Default Function


FlowOutput result = new SimpleflowEngine.Run(script,
                                            new object(),
                                            options,
                                            funcRegister);
```


## Cache Options
Specify cache options that suit your requirement. Cache options can be specified Simpleflow builder level as well context level.

```csharp
string id = "7bfd56c8ca354307b6cb9e0805a7ae4c";

var options = new FlowContextOptions { 
    // This will be used as cache key, if not specified then
    // it creates using hashing algorithm
    Id = id, 
    // This allows to reset the cache entry if script needs to recompile
    // ResetCache = true 
    CacheOptions = new CacheOptions { 
                        AbsoluteExpiration = System.DateTimeOffset.Now.AddHours(1),
                        SlidingExpiration = System.TimeSpan.FromMinutes(3),
                        // Default it uses MD5
                        HashingAlgToIdentifyScriptUniquely = "SHA256" 
                    }
};

FlowOutput result = new SimpleflowEngine.Run(script, new object(), options);
```

## Get Syntax Tree 
v1.0.3 
{: .fs-1 }

```csharp
SyntaxTree st = Simpleflow.Ast
                    .SimpleflowScript
                        .GetAbstractSyntaxTree("<your Simpleflow script here>")
```
This API provides abstract syntax tree data structure  of Simpleflow script. If there are any syntax errors, this method reports that error using SyntaxTree.Errors property.

