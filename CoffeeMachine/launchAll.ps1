$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-Command `$Host.UI.RawUI.WindowTitle=`'Coffee Machine`';dotnet run --project .\WebinarMassTransit.CoffeeMachine\WebinarMassTransit.CoffeeMachine.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-Command `$Host.UI.RawUI.WindowTitle=`'State Machine (Saga)`';dotnet run --project .\WebinarMassTransit.StateMachine\WebinarMassTransit.StateMachine.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-Command `$Host.UI.RawUI.WindowTitle=`'Topping Manager`';dotnet run --project .\WebinarMassTransit.ToppingManager\WebinarMassTransit.ToppingManager.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-Command `$Host.UI.RawUI.WindowTitle=`'Event Listener`';dotnet run --project .\WebinarMassTransit.EventListener\WebinarMassTransit.EventListener.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-Command `$Host.UI.RawUI.WindowTitle=`'Web API`';dotnet run --project .\WebinarMassTransit.Web\WebinarMassTransit.Web.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)