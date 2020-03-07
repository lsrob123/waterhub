function Get-Service-Name {
    return "app.healthfindings.web"
}

function Get-Executable-Name { 
    return "Blog.Web.exe" 
}

function Get-Description {
    return "Health Findings http://localhost:62490";
}

function Get-Publish-Folder {
    $serviceName = Get-Service-Name
    return "C:\applications\_publish_collections\" + $serviceName
}

function Get-Publish-Zip {
    $serviceName = Get-Service-Name
    return "C:\applications\_publish_collections\" + $serviceName + ".zip"
}

function Get-Publish-Unzip {
    $serviceName = Get-Service-Name
    return "C:\applications\_publish_collections\" + $serviceName + "_unzip"
}