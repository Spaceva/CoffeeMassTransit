$RootPath = $args[0]
write-host $RootPath
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Coffee Machine`';dotnet run --project $RootPath\CoffeeMassTransit.CoffeeMachine\CoffeeMassTransit.CoffeeMachine.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'State Machine (Saga)`';dotnet run --project $RootPath\CoffeeMassTransit.StateMachine\CoffeeMassTransit.StateMachine.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Topping Manager`';dotnet run --project $RootPath\CoffeeMassTransit.ToppingManager\CoffeeMassTransit.ToppingManager.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Event Listener`';dotnet run --project $RootPath\CoffeeMassTransit.EventListener\CoffeeMassTransit.EventListener.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Web API`';dotnet run --project $RootPath\CoffeeMassTransit.Web\CoffeeMassTransit.Web.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)