Param([string]$onlyWinStore, [string]$filters)

$rootDirectory = "%system.teamcity.build.workingDir%"
$configName = "DEV"
$vsConfigName = ""

$vstestconsolepath = "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
$dotcoverpath = "C:\BuildAgent\tools\dotCover\dotCover.exe"
$dotcovertargetexecutable = "/TargetExecutable=" + $vstestconsolepath
$dotcoveroutput = "/Output=" + $configName + "/coverage.dcvr"
$dotcoverfilters = "/Filters=" + $filters

$folder = get-location
#look for win store DLLs. If we find one we can't do code coverage on this test assembly
$winStoreDLLs = Get-ChildItem -Recurse -Force $folder.FullName -File | Where-Object { $_.Name -like "*Tests*.appx" } | Select-Object
foreach ($dll in $winStoreDLLs)
{
	$win8Arr += @($dll.FullName)
}

$testDlls = Get-ChildItem -Recurse -Force $folder.FullName -File | Where-Object { $_.Name -like "*Tests.dll" } | Select-Object
foreach ($dll in $testDlls)
{
	$arr += @($dll.FullName)
}

if ($win8Arr.length -gt 0)
{
    & $vstestconsolepath $win8Arr '/inIsolation' '/logger:TeamCityLogger'
}

# execute test DLLs without win store with dotCover
if ($arr.length -gt 0 -and $onlyWinStore -ne "onlyWinStore")
{
    #build up command for vstest console to pass inside of dotCover command
    $targetarguments = "/TargetArguments=" + $arr + " /inIsolation /logger:TeamCityLogger"

    #execute dotCover command with arguments
    & $dotcoverpath 'c' $dotcovertargetexecutable $targetarguments $dotcoveroutput $dotcoverfilters

    # pass message to teamcity to process code coverage
    "##teamcity[importData type='dotNetCoverage' tool='dotcover' path='" + $folder + "\" + $configName + "\coverage.dcvr']"
}