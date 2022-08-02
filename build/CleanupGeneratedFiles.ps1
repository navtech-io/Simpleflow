
#.\CleanupParserGeneratedFiles.ps1
# Prebuild event command: powershell -ExecutionPolicy Unrestricted .\..\..\build_script\CleanupParserGeneratedFiles.ps1


# change public to internal in Simpleflow* classes

$file = "..\src\Simpleflow\Parser\SimpleflowParserBaseListener.cs"
(Get-Content $file) -Replace 'public partial class', 'internal partial class' | Set-Content $file

$file = "..\src\Simpleflow\Parser\SimpleflowParserBaseVisitor.cs"
(Get-Content $file) -Replace 'public partial class', 'internal partial class' | Set-Content $file

$file = "..\src\Simpleflow\Parser\SimpleflowLexer.cs"
(Get-Content $file) -Replace 'public partial class', 'internal partial class' | Set-Content $file

$file = "..\src\Simpleflow\Parser\SimpleflowParserListener.cs"
(Get-Content $file) -Replace 'public interface', 'internal interface' | Set-Content $file

$file = "..\src\Simpleflow\Parser\SimpleflowParser.cs"
(Get-Content $file) -Replace 'public partial class', 'internal partial class' | Set-Content $file

$file = "..\src\Simpleflow\Parser\SimpleflowParserVisitor.cs"
(Get-Content $file) -Replace 'public interface', 'internal interface' | Set-Content $file