
#.\CleanupParserGeneratedFiles.ps1
# Prebuild event command: powershell -ExecutionPolicy Unrestricted .\..\..\build_script\CleanupParserGeneratedFiles.ps1

# Remove unncessary files from parser except that starts with Simpleflow*

Remove-Item ..\src\Simpleflow\Parser\Common*.*
Remove-Item ..\src\Simpleflow\Parser\Expression*.*
Remove-Item ..\src\Simpleflow\Parser\Json*.*
Remove-Item ..\src\Simpleflow\Parser\Predicate*.*

# change public to internal in Simpleflow* classes

$file = "..\src\Simpleflow\Parser\SimpleflowBaseListener.cs"
(Get-Content $file) -Replace 'public partial class', 'internal partial class' | Set-Content $file

$file = "..\src\Simpleflow\Parser\SimpleflowBaseVisitor.cs"
(Get-Content $file) -Replace 'public partial class', 'internal partial class' | Set-Content $file

$file = "..\src\Simpleflow\Parser\SimpleflowLexer.cs"
(Get-Content $file) -Replace 'public partial class', 'internal partial class' | Set-Content $file

$file = "..\src\Simpleflow\Parser\SimpleflowListener.cs"
(Get-Content $file) -Replace 'public interface', 'internal interface' | Set-Content $file

$file = "..\src\Simpleflow\Parser\SimpleflowParser.cs"
(Get-Content $file) -Replace 'public partial class', 'internal partial class' | Set-Content $file

$file = "..\src\Simpleflow\Parser\SimpleflowVisitor.cs"
(Get-Content $file) -Replace 'public interface', 'internal interface' | Set-Content $file