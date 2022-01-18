$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Coffee Machine`';dotnet run --project .\CoffeeMassTransit.CoffeeMachine\CoffeeMassTransit.CoffeeMachine.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Saga`';dotnet run --project .\CoffeeMassTransit.ConsumerSaga\CoffeeMassTransit.ConsumerSaga.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Topping Manager`';dotnet run --project .\CoffeeMassTransit.ToppingManager\CoffeeMassTransit.ToppingManager.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Event Listener`';dotnet run --project .\CoffeeMassTransit.EventListener\CoffeeMassTransit.EventListener.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-NoExit -Command `$Host.UI.RawUI.WindowTitle=`'Web API`';dotnet run --project .\CoffeeMassTransit.Web\CoffeeMassTransit.Web.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)