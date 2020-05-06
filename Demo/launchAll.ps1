$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-Command `$Host.UI.RawUI.WindowTitle=`'Service A`';dotnet run --project .\WebinarMassTransit.DemoServiceA\WebinarMassTransit.DemoServiceA.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
$StartInfo = new-object System.Diagnostics.ProcessStartInfo
$StartInfo.FileName = "$pshome\powershell.exe"
$StartInfo.Arguments = "-Command `$Host.UI.RawUI.WindowTitle=`'Service B`';dotnet run --project .\WebinarMassTransit.DemoServiceB\WebinarMassTransit.DemoServiceB.csproj --no-build"
[System.Diagnostics.Process]::Start($StartInfo)
