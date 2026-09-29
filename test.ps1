param([switch]$NoRestore)

$ErrorActionPreference = 'Stop'
$projectRoot = $PSScriptRoot
$localDotnet = Join-Path $projectRoot '.tools/dotnet/dotnet.exe'

if (Test-Path -LiteralPath $localDotnet) {
    $dotnetCommand = $localDotnet
    $env:DOTNET_ROOT = Split-Path -Parent $localDotnet
    $env:DOTNET_ROOT_X64 = $env:DOTNET_ROOT
} else {
    $dotnetCommand = (Get-Command dotnet -ErrorAction Stop).Source
}

$env:DOTNET_CLI_HOME = Join-Path $projectRoot '.tools/cli'
$env:NUGET_PACKAGES = Join-Path $projectRoot '.tools/packages'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_NOLOGO = '1'
$solution = Join-Path $projectRoot 'AutoService.slnx'

if (-not $NoRestore) {
    & $dotnetCommand restore $solution
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

& $dotnetCommand format $solution --verify-no-changes --no-restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

& $dotnetCommand test $solution --configuration Release --no-restore --logger 'trx;LogFileName=tests.trx' --results-directory (Join-Path $projectRoot 'TestResults')
exit $LASTEXITCODE
