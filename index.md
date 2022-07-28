---
layout: default
title: Get Started
nav_order: 1
permalink: /
---

# <img src="https://raw.githubusercontent.com/navtech-io/Simpleflow/develop/src/Simpleflow/PackageIcon.png" style="width:80px;vertical-align:middle" > Simpleflow .NET
{: .fs-9 }

Simpleflow is a .NET libraray to build dynamic rules and workflows with intuitive scripting language. Simpleflow allows you to pass an argument to the script and can access registered methods in the script securely. Methods can be registered as activities with Simpleflow engine, which is extensible to enrich or monitor the execution flow. Simpleflow is secure and efficient to run dynamic rules and workflows. 
{: .fs-6 .fw-300 }

[![NuGet version (Simpleflow)](https://img.shields.io/nuget/vpre/Simpleflow?style=for-the-badge)](https://www.nuget.org/packages/Simpleflow/)


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
FlowOutput result = SimpleflowEngine.Run(flowScript, 
                                        new {UniversalId = 2, New=true, Verified=false});
// Access result
Console.WriteLine(result.Messages[0]); 
var scriptExecutionTime =  result.Output["today"];

```

**Output**

```
Hello, World! 🌄

```
Please see [this](docs/examples/) example with most of the simpleflow script features.

---

* [Script Outline](docs/simpleflow-script-reference/#script-outline)
* [Reference](docs/simpleflow-script-reference/#script-outline)
* [API](docs/api/)
* [Examples](docs/examples/)
* [Limitations](docs/simpleflow-script-reference/#limitations)


