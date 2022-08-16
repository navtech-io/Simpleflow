---
layout: default
title: Indexer
parent: Examples
---

# Examples

### Indexer
Since v1.0.4
{: .fs-1 }

```csharp
let name = arg["name"] 
output name
```
### Execute
```csharp

// Execute 
var result = SimpleflowEngine.Run(script, new Dictionary<string, string>
                                            {
                                               {"name", "John" },
                                               {"city", "LA" },
                                               {"state", "California" }
                                            });

var name =  result.Output["name"];
```

### List of integers and objects

```csharp
let values  = [1 ,2 ,3 , 4 ]
let objects = [1 , "test", `test {values[ 1 ]}`, $GetCurrentDate() ]
output objects
output values
```
### Execute
```csharp

// Execute 
var result = SimpleflowEngine.Run(script, new object());
```
