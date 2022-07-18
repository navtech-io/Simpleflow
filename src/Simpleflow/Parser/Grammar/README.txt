
### vs code settings	

```
    "antlr4.generation": {
		"package": "Simpleflow.Parser",
		"alternativeJar": "antlr-4.10.1-complete.jar",
		"outputDir": "Parser",
		"mode": "external",
		"language": "CSharp",
		"listeners": true,
		"visitors": true
	}
```

antlr-4.10.1-complete.jar  -o code -visitor -package Simpleflow.Parser -Dlanguage=CSharp -Xlog -Werror Simpleflow.g4