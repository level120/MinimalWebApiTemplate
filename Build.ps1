using namespace System.Management.Automation
using namespace System.Management.Automation.Language

# Set global format culture

if ($host.Name -eq 'ConsoleHost')
{
    Import-Module PSReadLine
}

[Threading.Thread]::CurrentThread.CurrentCulture = 'en-US'


# Build OS Checking

Write-Host "Build machine os: '$([System.Environment]::OSVersion.VersionString)'" -ForegroundColor DarkGreen


# Build Tools Checking

if (Get-Command -Name "dotnet" -errorAction SilentlyContinue) {
  Write-Host "Detect 'dotnet' command: " -ForegroundColor DarkBlue -NoNewline

  $sdkVersions = (& dotnet --list-sdks).Split([System.Environment]::NewLine) | ForEach-Object -Process {
      New-Object System.Version($_.Split()[0].Split('-')[0])
  }

  $supportSdks = $sdkVersions | Group-Object -Propert Major, Minor | ForEach-Object -Process {
      Join-String -InputObject $_.Group.Major[0], $_.Group.Minor[0] -Separator "."
  } | Join-String -SingleQuote -Separator ','

  Write-Host $supportSdks -ForegroundColor DarkGreen
}


# Set Build Paths

[string] $RootPath = $PSScriptRoot
[string] $Configuration = "Release"
[string] $ArtifactPath = "$RootPath/artifacts"
[string] $PublishDir = Join-Path -Path $ArtifactPath -ChildPath "publish/WebApi/release"


# Build

& dotnet publish -c $Configuration


# Check Build Result

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "Build succeeded" -ForegroundColor Green


# Open Publish Directory

Invoke-item -Path $PublishDir
