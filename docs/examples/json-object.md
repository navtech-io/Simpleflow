---
layout: default
title: JSON Object
parent: Examples
---

# JSON Object

```csharp
$SaveConfig(config: { 
               database: 'config',
               credential: {
                     user: 'test',
                     token: arg.token
               },
               server: 'localhost'
           })
```
### Execute

```csharp
// Register custom function
var register = new FunctionRegister()
    .Add("SaveConfig", (Action<Config>)SaveConfig);

// Execute 
SimpleflowEngine.Run(script, 
                     new {Token = "e2fc714c4727ee9395f324cd2e7f331f"},
                     register);
```

```csharp
static void SaveConfig(Config config)
{
    // ...
}
```
