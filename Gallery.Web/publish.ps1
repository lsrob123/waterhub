. ".\appsettings.ps1"

function Publish {
    Write-Host "Publishing to $publishFolder..."

    if (Test-Path -Path $publishFolder ) { 
        $existingFiles = $publishFolder + "\*"
        Remove-Item -Path $existingFiles -Recurse
    }
    dotnet publish .\ --force -o $publishFolder
}

$publishFolder = Get-Publish-Folder
$publishZip = Get-Publish-Zip

Publish
$sourcePath = $publishFolder + "\*"
Compress-Archive -Path $sourcePath -DestinationPath $publishZip -Force