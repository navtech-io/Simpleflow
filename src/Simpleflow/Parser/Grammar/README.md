
### vs code settings	

```
    "antlr4.generation": {
		"package": "Simpleflow.Parser",
		"alternativeJar": "C:\\Users\\DELL\\Downloads\\antlr-4.10.1-complete.jar",
		"outputDir": "C:\\Navtech\\Opensource\\Simpleflow\\src\\Simpleflow\\Parser",
		"mode": "external",
		"language": "CSharp",
		"listeners": true,
		"visitors": true
	}
```


C:\Users\DELL\Downloads\antlr-4.10.1-complete.jar  -o code -visitor -package Simpleflow.Parser -Dlanguage=CSharp -Xlog -Werror Simpleflow.g4