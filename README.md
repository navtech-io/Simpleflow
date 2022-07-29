
# Simpleflow

Simpleflow is a lightweight dynamic rule engine to build workflow with intuitive script concepts. Simpleflow allows access to the process objects or methods in script securely. Methods can be registered as activities with Simpleflow engine, which is extensible to enrich or monitor the execution flow. Simpleflow is secure and efficient to run dynamic rules and workflow. 


[![NuGet version (Simpleflow)](https://img.shields.io/nuget/vpre/Simpleflow?style=for-the-badge)](https://www.nuget.org/packages/Simpleflow/) [![Github Workflow (Simpleflow)](https://img.shields.io/github/workflow/status/navtech-io/simpleflow/ci?style=for-the-badge)](https://github.com/navtech-io/Simpleflow/actions)

![Simpleflow .NET Rule and Workflow Engine](https://raw.githubusercontent.com/navtech-io/Simpleflow/develop/src/Simpleflow/PackageIcon.png)


## Get Started

Install package via Nuget. Using the NuGet package manager console within Visual Studio run the following command:

```powershell
Install-Package Simpleflow	
```

**Try the following Simpleflow script:**

```csharp
using Simpleflow;

// Simpleflow Script
var flowScript = 
@" 
    let text  = ""Hello, World! 🌄""
    let today = $GetCurrentDateTime ( timezone: ""Eastern Standard Time"" )

    /* comment: check condition  */
    rule when arg.UniversalId == 2 and (arg.New or arg.Verified)  then
         message text
    end rule
    
    output today
";

// Execute Script
FlowOutput result = SimpleflowEngine.Run(flowScript, new {UniversalId = 2, New=true, Verified=false} );

// Access result
Console.WriteLine(result.Messages[0]); 
Console.WriteLine(result.Output["today"]);

```
**Output**

```
Hello, World! 🌄
<current system date and time>
```

[Please see the full documentation](https://navtech-io.github.io/Simpleflow/) on how to use this library.


## License
The code is available as open source under the terms of the [Apache 2.0](https://github.com/navtech-io/Simpleflow/blob/main/LICENSE) License. 
Please see the LICENSE file in this repository.

