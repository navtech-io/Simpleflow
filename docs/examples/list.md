---
layout: default
title: List
parent: Examples
---

# List

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
