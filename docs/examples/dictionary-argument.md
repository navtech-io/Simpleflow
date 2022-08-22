---
layout: default
title: Dictionary as an argument
parent: Examples
---

# Examples
### Send dictionary as an argument and access value based on key

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

