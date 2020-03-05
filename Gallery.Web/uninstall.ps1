. ".\appsettings.ps1"

$serviceName = Get-Service-Name

If (Get-Service $serviceName -ErrorAction SilentlyContinue) {
    If ((Get-Service $serviceName).Status -eq 'Running') {
        Write-Host "Stopping $serviceName..."
        Stop-Service $serviceName
        Write-Host "$serviceName stopped."
    }
    
    sc.exe delete $serviceName
    Write-Host "$serviceName removed."
}
else {
    Write-Host "Service $serviceName not found."
}

