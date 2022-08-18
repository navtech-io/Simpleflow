---
layout: default
title: Array and Dictionary
parent: Examples
---

# Examples


### Array

```csharp
let values  = [1 ,2 ,3 , 4 ]
let objects = [1 , "test", `test {values[ 1 ]}`, $GetCurrentDate() ]
output objects
output values
```
### Execute array example script
```csharp

// Execute 
var result = SimpleflowEngine.Run(script, new object());
```

### Send dictionary as an argument and access value based on key
Since v1.0.4
{: .fs-1 }

```csharp
let name = arg["name"] 
output name
```
### Execute dictionary example script

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

