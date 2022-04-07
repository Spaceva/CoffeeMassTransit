$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Main Orchestration`';dotnet run --project .\CoffeeMassTransit.StateMachine\CoffeeMassTransit.SubOrchestration.CoffeeOrderStateMachine.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Sub Orchestration`';dotnet run --project .\CoffeeMassTransit.SubOrchestration.CoffeeMachine\CoffeeMassTransit.SubOrchestration.CoffeeStateMachine.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Sub Orchestration Consumers`';dotnet run --project .\CoffeeMassTransit.SubOrchestration.Consumers\CoffeeMassTransit.SubOrchestration.Consumers.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'UI`';dotnet run --project ..\CoffeeMachine\CoffeeMassTransit.Web\CoffeeMassTransit.Web.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)