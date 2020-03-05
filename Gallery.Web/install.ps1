. ".\appsettings.ps1"

function Publish {
    Write-Host "Publishing to $publishFolder..."

    if (Test-Path -Path $publishFolder ) { 
        $existingFiles = $publishFolder + "\*"
        Remove-Item -Path $existingFiles -Recurse
    }
    dotnet publish .\ --force -o $publishFolder
}

function Copy-Folder-Files {
    Param ([string]$source, [string]$destination)

    if ( -Not (Test-Path -Path $destination ) ) {
        New-Item -ItemType directory -Path $destination
        Write-Host "Folder $destination created."
    }

    Write-Host "Removing files in $destination."
    Remove-Item -Path $destination -Recurse

    Write-Host "Copying files from $source to $destination"
    Copy-item -Path $source -Destination $destination -Force -Recurse
}

# $files is the wildcard path to the files.
function Backup-Files {
    Param ([string]$existingFileFolder, [string]$backupFolder)

    New-Item -ItemType directory -Path $backupFolder
    Write-Host "Backing up files from $existingFileFolder to $backupFolder..."
    Copy-item -Path $existingFileFolder -Destination $backupFolder -Force -Recurse 
}

function Copy-Service-Files {
    $backupTime = Get-Date -Format o | ForEach-Object { $_ -replace ":", "." }
    $backupFolder = $rootFolder + "\_backups\" + $serviceName + "\" + $backupTime+"\"
    Backup-Files -existingFileFolder $destinationFolder -backupFolder $backupFolder

    Copy-Folder-Files -source $publishFolder -destination $destinationFolder
}

$serviceName = Get-Service-Name
$executableName = Get-Executable-Name
$description = Get-Description

$rootFolder = "c:\applications";
$source = Get-Location
$destinationFolder = $rootFolder + "\" + $serviceName
$executablePath = $destinationFolder + "\" + $executableName
$publishFolder = Get-Publish-Folder

Publish

If (Get-Service $serviceName -ErrorAction SilentlyContinue) {
    If ((Get-Service $serviceName).Status -eq 'Running') {
        Write-Host "Stopping $serviceName..."
        Stop-Service $serviceName
        Write-Host "$serviceName stopped."
    }
    Copy-Service-Files
}
Else {
    Copy-Service-Files
    Write-Host "Creating service $serviceName with $executablePath."
    New-Service -Name $serviceName -BinaryPathName $executablePath -DisplayName $serviceName -Description $description -StartupType Automatic
    Write-Host "Service $serviceName created."
}

Start-Service -Name $serviceName
Write-Host "Service $serviceName started."