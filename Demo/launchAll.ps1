$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-Command `$Host.UI.RawUI.WindowTitle=`'Service A`';dotnet run --project .\CoffeeMassTransit.DemoServiceA\CoffeeMassTransit.DemoServiceA.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-Command `$Host.UI.RawUI.WindowTitle=`'Service B`';dotnet run --project .\CoffeeMassTransit.DemoServiceB\CoffeeMassTransit.DemoServiceB.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
